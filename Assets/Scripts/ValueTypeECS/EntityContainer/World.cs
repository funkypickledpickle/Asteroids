using System;
using System.Collections;
using System.Collections.Generic;
using Asteroids.Configuration;
using Asteroids.ValueTypeECS.Components;
using Asteroids.ValueTypeECS.DataContainers;
using Asteroids.ValueTypeECS.Delegates;
using Asteroids.ValueTypeECS.ECSTypes;
using Asteroids.ValueTypeECS.Entities;
using UnityEngine.Pool;

namespace Asteroids.ValueTypeECS.EntityContainer
{
    public class World : IEnumerable<int>
    {
        public event Action WillClear;

        public event ActionReference<Entity> EntityCreated;
        public event ActionReference<Entity> EntityRemoved;

        public event ActionReferenceValue<Entity, ComponentKey> ComponentCreated;
        public event ActionReferenceValue<Entity, ComponentKey> ComponentRemoved;

        private readonly ActionReferenceValue<Entity, ComponentKey> _handleComponentCreated;
        private readonly ActionReferenceValue<Entity, ComponentKey> _handleComponentRemoved;
        private readonly ECSTypeKeyEqualityComparer _typeKeyEqualityComparer = new ECSTypeKeyEqualityComparer();

        private readonly GamePreloadConfiguration _preloadConfiguration;
        private readonly IComponentsContainer _componentsContainer;

        private readonly UnorderedSegmentedList<Entity> _entities;

        public readonly ContainersConfiguration ContainersConfiguration;

        public World(ContainersConfiguration containersConfiguration, GamePreloadConfiguration preloadConfiguration, IComponentsContainer componentsContainer)
        {
            _handleComponentCreated = HandleComponentCreated;
            _handleComponentRemoved = HandleComponentRemoved;

            ContainersConfiguration = containersConfiguration;
            _preloadConfiguration = preloadConfiguration;
            _componentsContainer = componentsContainer;

            _entities = new UnorderedSegmentedList<Entity>(containersConfiguration.ArraySizeForEntitySegmentedList);
            _componentsContainer = componentsContainer;
        }

        public void TryPreload()
        {
            List<int> ids = ListPool<int>.Get();
            for (int i = 0; i < _preloadConfiguration.PreloadedEntitiesQuantity; i++)
            {
                ref var container = ref _entities.Reserve();
                if (!container.IsInitialized)
                {
                    container.Initialize(new Entity(container.Index, _componentsContainer,
                        new Dictionary<ECSTypeKey, ComponentKey>(ContainersConfiguration.ComponentsDictionarySizeInEntity, _typeKeyEqualityComparer),
                        _handleComponentCreated, _handleComponentRemoved));
                }

                ids.Add(container.Index);
            }

            foreach (int id in ids)
            {
                _entities.Free(id);
            }

            ids.Clear();
            ListPool<int>.Release(ids);
        }

        public ref Entity CreateEntity()
        {
            ref var container = ref _entities.Reserve();
            if (!container.IsInitialized)
            {
                container.Initialize(new Entity(container.Index, _componentsContainer,
                    new Dictionary<ECSTypeKey, ComponentKey>(ContainersConfiguration.ComponentsDictionarySizeInEntity, _typeKeyEqualityComparer),
                    _handleComponentCreated, _handleComponentRemoved));
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
