using System;
using System.Collections.Generic;
using Asteroids.ValueTypeECS.Components;
using Asteroids.ValueTypeECS.Delegates;
using Asteroids.ValueTypeECS.ECSTypes;

namespace Asteroids.ValueTypeECS.Entities
{
    public struct ComponentKey
    {
        public int Index { get; }
        public ECSTypeKey TypeKey { get; }

        public ComponentKey(int index, ECSTypeKey typeKey)
        {
            Index = index;
            TypeKey = typeKey;
        }

        public override string ToString()
        {
            return $"ComponentKey({Index}, {ECSTypeService.GetSystemType(TypeKey).Name})";
        }
    }

    public struct Entity : IECSObject
    {
        private IComponentsContainer _componentsContainer;
        private Dictionary<ECSTypeKey, ComponentKey> _components;
        private ActionReferenceValue<Entity, ComponentKey> _componentCreated;
        private ActionReferenceValue<Entity, ComponentKey> _componentRemoved;
        public readonly int Id;

        public Entity(int id, IComponentsContainer componentsContainer, Dictionary<ECSTypeKey, ComponentKey> components, ActionReferenceValue<Entity, ComponentKey> componentCreated, ActionReferenceValue<Entity, ComponentKey> componentRemoved)
        {
            Id = id;
            _componentsContainer = componentsContainer;
            _components = components;
            _componentCreated = componentCreated;
            _componentRemoved = componentRemoved;
        }

        public bool HasComponent<TComponent>() where TComponent : struct, IECSComponent
        {
            var typeKey = ECSTypeService.GetType<TComponent>();
            return _components.ContainsKey(typeKey);
        }

        public void CreateComponent<TComponent>(TComponent source) where TComponent : struct, IECSComponent
        {
            if (HasComponent<TComponent>())
            {
                throw new ArgumentException($"Entity {Id} already has component type of {typeof(TComponent)}");
            }

            var typeKey = ECSTypeService.GetType<TComponent>();
            ref var component = ref _componentsContainer.CreateComponent<TComponent>(out var index);
            var componentKey = new ComponentKey(index, typeKey);
            _components.Add(typeKey, componentKey);
            component = source;
            _componentCreated(ref this, componentKey);
        }

        public void CreateComponent<TComponent>() where TComponent : struct, IECSComponent
        {
            CreateComponent<TComponent>(default);
        }

        public ref TComponent GetComponent<TComponent>() where TComponent : struct, IECSComponent
        {
            var typeKey = ECSTypeService.GetType<TComponent>();
            if (!_components.ContainsKey(typeKey))
            {
                throw new ArgumentException($"The entity_{Id} doesn't have the component type of {typeof(TComponent)}");
            }

            return ref _componentsContainer.GetComponent<TComponent>(_components[typeKey].Index);
        }

        public void RemoveComponent<TComponent>() where TComponent : struct, IECSComponent
        {
            var typeKey = ECSTypeService.GetType<TComponent>();
            RemoveComponent(typeKey);
        }

        public void RemoveComponent(ECSTypeKey typeKey)
        {
            if (!_components.ContainsKey(typeKey))
            {
                throw new InvalidOperationException();
            }

            var componentKey = _components[typeKey];
            var componentIndex = componentKey.Index;
            _components.Remove(typeKey);
            _componentRemoved(ref this, componentKey);
            _componentsContainer.RemoveComponent(typeKey, componentIndex);
        }

        public void Destroy()
        {
            var keysCollection = _components.Keys;
            ECSTypeKey typeKey = default;
            while (keysCollection.Count > 0)
            {
                foreach (var value in keysCollection)
                {
                    typeKey = value;
                    break;
                }

                RemoveComponent(typeKey);
            }
        }

        public void Reset()
        {
            _components.Clear();
        }
    }
}
