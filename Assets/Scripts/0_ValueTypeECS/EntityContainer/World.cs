using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Components;
using Asteroids.ValueTypeECS.DataContainers;
using Asteroids.ValueTypeECS.ECSTypes;
using Asteroids.ValueTypeECS.Entities;

namespace Asteroids.ValueTypeECS.EntityContainer
{
    public interface IEntityObserver
    {
        void EntityCreated(ref Entity entity);
        void EntityRemoved(ref Entity entity);
    }

    public interface IComponentObserver
    {
        void ComponentCreated(ref Entity entity, ComponentKey key);
        void ComponentRemoved(ref Entity entity, ComponentKey key);
    }

    public class World
    {
        private class EntityCollection : IEnumerable<int>
        {
            private UnorderedSegmentedList<Entity> _entities;

            public EntityCollection(UnorderedSegmentedList<Entity> entities)
            {
                _entities = entities;
            }

            public IEnumerator<int> GetEnumerator()
            {
                foreach (var index in _entities)
                {
                    yield return index;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private readonly UnorderedSegmentedList<Entity> _entities;
        private readonly IComponentsContainer _componentsContainer;

        private readonly List<IEntityObserver> _observers = new List<IEntityObserver>();
        private readonly List<IComponentObserver> _entityObserver = new List<IComponentObserver>();

        public World(int segmentedListCapacity, IComponentsContainer componentsContainer)
        {
            _componentsContainer = componentsContainer;
            _entities = new UnorderedSegmentedList<Entity>(segmentedListCapacity);
            _componentsContainer = componentsContainer;
        }

        public ref Entity CreateEntity()
        {
            ref var container = ref _entities.Reserve();
            if (!container.Initialized)
            {
                container.Value = new Entity(container.Index, _componentsContainer, new Dictionary<ECSTypeKey, ComponentKey>(new ECSTypeKeyEqualityComparer()), ComponentCreatedHandler, ComponentRemovedHandler);
                container.Initialized = true;
            }

            RaiseEntityCreated(ref container.Value);
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
            RaiseEntityRemoved(ref _entities.GetReservedValue(id).Value);
            _entities.Free(id);
        }

        private void RaiseEntityCreated(ref Entity entity)
        {
            LogEvent(entity.Id, $"EntityCreated");
            foreach (var observer in _observers)
            {
                observer.EntityCreated(ref entity);
            }
        }

        private void RaiseEntityRemoved(ref Entity entity)
        {
            LogEvent(entity.Id, $"EntityRemoved");
            foreach (var observer in _observers)
            {
                observer.EntityRemoved(ref entity);
            }
        }

        private void RaiseComponentCreated(ref Entity entity, ComponentKey key)
        {
            LogEvent(entity.Id, $"ComponentCreated: {key}");
            foreach (var observer in _entityObserver)
            {
                observer.ComponentCreated(ref entity, key);
            }
        }

        private void RaiseComponentRemoved(ref Entity entity, ComponentKey key)
        {
            LogEvent(entity.Id, $"ComponentRemoved: {key}");
            foreach (var observer in _entityObserver)
            {
                observer.ComponentRemoved(ref entity, key);
            }
        }

        public void Subscribe(IEntityObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IEntityObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Subscribe(IComponentObserver observer)
        {
            _entityObserver.Add(observer);
        }

        public void Unsubscribe(IComponentObserver observer)
        {
            _entityObserver.Remove(observer);
        }

        private void ComponentCreatedHandler(ref Entity entity, ComponentKey key)
        {
            RaiseComponentCreated(ref entity, key);
        }

        private void ComponentRemovedHandler(ref Entity entity, ComponentKey key)
        {
            RaiseComponentRemoved(ref entity, key);
        }

        [Conditional("LOG_ECS_EVENTS")]
        private void LogEvent(int id, string message)
        {
            this.Log(LogCategory.ECS, $"Entity {id} Action: {message}");
        }
    }
}
