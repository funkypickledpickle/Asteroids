using System;
using System.Collections.Generic;
using System.Linq;
using Asteroids.ValueTypeECS.Delegates;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityContainer;

namespace Asteroids.ValueTypeECS.EntityGroup
{
    public class EntityGroup : IComponentObserver, IEntityObserver
    {
        public readonly World World;
        private readonly FunctionReference<Entity, bool> _entityCondition;

        private ActionReference<Entity> _entityAddedObservers;
        private ActionReference<Entity> _entityRemovedObservers;

        private List<int> _ids;
        public int Count => _ids.Count;

        private int _enumeratorIndex;

        public IReadOnlyList<int> IdsList => _ids;

        public EntityGroup(World world, FunctionReference<Entity, bool> entityCondition)
        {
            World = world;
            _entityCondition = entityCondition;
            _ids = new List<int>();

            World.Subscribe((IComponentObserver)this);
            World.Subscribe((IEntityObserver)this);
        }

        public void SubscribeToEntityAddedEvent(ActionReference<Entity> entityAddedActionReference)
        {
            _entityAddedObservers += entityAddedActionReference;
        }

        public void UnsubscribeFromEntityAddedEvent(ActionReference<Entity> entityAddedActionReference)
        {
            _entityAddedObservers -= entityAddedActionReference;
        }

        public void SubscribeToEntityRemovedEvent(ActionReference<Entity> entityRemovedActionReference)
        {
            _entityRemovedObservers += entityRemovedActionReference;
        }

        public void UnsubscribeFromEntityRemovedEvent(ActionReference<Entity> entityRemovedActionReference)
        {
            _entityRemovedObservers -= entityRemovedActionReference;
        }

        private void EntityUpdatedHandler(ref Entity entity)
        {
            var indexOfEntity = _ids.IndexOf(entity.Id);
            var hasEntity = indexOfEntity != -1;
            var isConditionCompleted = _entityCondition(ref entity);

            if (isConditionCompleted && !hasEntity)
            {
                AddEntity(ref entity);
            }
            else if (!isConditionCompleted && hasEntity)
            {
                RemoveEntity(indexOfEntity, ref entity);
            }
        }

        void IComponentObserver.ComponentCreated(ref Entity entity, ComponentKey key)
        {
            EntityUpdatedHandler(ref entity);
        }

        void IComponentObserver.ComponentRemoved(ref Entity entity, ComponentKey key)
        {
            EntityUpdatedHandler(ref entity);
        }

        void IEntityObserver.EntityCreated(ref Entity entity)
        {
            if (_entityCondition(ref entity))
            {
                AddEntity(ref entity);
            }
        }

        void IEntityObserver.EntityRemoved(ref Entity entity)
        {
            var index = _ids.IndexOf(entity.Id);
            if (index != -1)
            {
                RemoveEntity(index, ref entity);
            }
        }

        public EntityGroup GetEnumerator()
        {
            Reset();
            return this;
        }

        private void AddEntity(ref Entity entity)
        {
            _ids.Add(entity.Id);
            RaiseEntityCreatedEvent(ref entity);
        }

        private void RemoveEntity(int index, ref Entity entity)
        {
            if (index >= _enumeratorIndex)
            {
                _enumeratorIndex--;
            }

            _ids.RemoveAt(index);
            RaiseEntityRemovedEvent(ref entity);
        }

        private void RaiseEntityCreatedEvent(ref Entity entity)
        {
            _entityAddedObservers?.Invoke(ref entity);
        }

        private void RaiseEntityRemovedEvent(ref Entity entity)
        {
            _entityRemovedObservers?.Invoke(ref entity);
        }

        public bool MoveNext()
        {
            if (++_enumeratorIndex >= _ids.Count)
            {
                return false;
            }

            return true;
        }

        public void Reset()
        {
            _enumeratorIndex = -1;
        }

        public int Current => _ids[_enumeratorIndex];
    }
}
