using System;
using System.Collections;
using System.Collections.Generic;
using Asteroids.Configuration;
using Asteroids.ValueTypeECS.Components;
using Asteroids.ValueTypeECS.DataContainers;
using Asteroids.ValueTypeECS.Delegates;
using Asteroids.ValueTypeECS.ECSTypes;
using Asteroids.ValueTypeECS.Entities;

namespace Asteroids.ValueTypeECS.EntityContainer
{
    public class World : IEnumerable<int>
    {
        public event Action WillClear;

        private readonly UnorderedSegmentedList<Entity> _entities;
        private readonly IComponentsContainer _componentsContainer;

        public event ActionReference<Entity> EntityCreated;
        public event ActionReference<Entity> EntityRemoved;

        public event ActionReferenceValue<Entity, ComponentKey> ComponentCreated;
        public event ActionReferenceValue<Entity, ComponentKey> ComponentRemoved;

        public World(ContainersConfiguration configuration, IComponentsContainer componentsContainer)
        {
            _componentsContainer = componentsContainer;
            _entities = new UnorderedSegmentedList<Entity>(configuration.ArraySizeForEntitySegmentedList);
            _componentsContainer = componentsContainer;
        }

        public ref Entity CreateEntity()
        {
            ref var container = ref _entities.Reserve();
            if (!container.IsInitialized)
            {
                container.Initialize(new Entity(container.Index, _componentsContainer,
                    new Dictionary<ECSTypeKey, ComponentKey>(new ECSTypeKeyEqualityComparer()),
                    HandleComponentCreated, HandleComponentRemoved));
            }

            EntityCreated?.Invoke(ref container.Value);
            return ref container.Value;
        }

        public ref Entity GetEntity(int id)
        {
            return ref _entities.GetReservedValue(id).Value;
        }

        public void RemoveEntity(int id)
        {
            ref var entity = ref _entities.GetReservedValue(id).Value;
            entity.Destroy();
            EntityRemoved?.Invoke(ref _entities.GetReservedValue(id).Value);
            _entities.Free(id);
        }

        public void Clear()
        {
            WillClear?.Invoke();

            foreach (var index in this)
            {
                GetEntity(index).Reset();
            }

            _entities.Clear();
            _componentsContainer.Reset();
        }

        public SegmentedListEnumerator GetEnumerator()
        {
            return _entities.GetEnumerator();
        }

        IEnumerator<int> IEnumerable<int>.GetEnumerator()
        {
            foreach (var index in _entities)
            {
                yield return index;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (this as IEnumerable<int>).GetEnumerator();
        }

        private void HandleComponentCreated(ref Entity entity, ComponentKey key)
        {
            ComponentCreated?.Invoke(ref entity, key);
        }

        private void HandleComponentRemoved(ref Entity entity, ComponentKey key)
        {
            ComponentRemoved?.Invoke(ref entity, key);
        }
    }
}
