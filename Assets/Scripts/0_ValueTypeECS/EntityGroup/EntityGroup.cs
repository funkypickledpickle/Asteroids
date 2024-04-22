using System;
using System.Collections;
using System.Collections.Generic;
using Asteroids.ValueTypeECS.Delegates;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityContainer;
using UnityEngine.Pool;

namespace Asteroids.ValueTypeECS.EntityGroup
{
    public class EntityGroup : IDisposable
    {
        public readonly World World;
        private readonly FunctionReference<Entity, bool> _entityCondition;

        public event ActionReference<Entity> EntityAdded;
        public event ActionReference<Entity> EntityRemoved;

        private List<int> _ids;
        public int Count => _ids.Count;

        public IReadOnlyList<int> IdsList => _ids;

        public EntityGroup(World world, FunctionReference<Entity, bool> entityCondition)
        {
            _handleActiveEnumeratorDisposed = HandleActiveEnumeratorDisposed;

            World = world;
            _entityCondition = entityCondition;

            _ids = new List<int>();
            foreach (int entityIndex in world)
            {
                ref var entity = ref world.GetEntity(entityIndex);
                if (entityCondition(ref entity))
                {
                    _ids.Add(entity.Id);
                }
            }

            world.WillClear += WorldWillClearHandler;
            world.EntityCreated += HandleEntityCreated;
            world.EntityRemoved += HandleEntityRemoved;
            world.ComponentCreated += HandleComponentCreated;
            world.ComponentRemoved += HandleComponentRemoved;
        }

        public void Dispose()
        {
            var world = World;
            world.WillClear -= WorldWillClearHandler;
            world.EntityCreated -= HandleEntityCreated;
            world.EntityRemoved -= HandleEntityRemoved;
            world.ComponentCreated -= HandleComponentCreated;
            world.ComponentRemoved -= HandleComponentRemoved;

            _ids = null;
        }

        private void WorldWillClearHandler()
        {
            _ids.Clear();
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

        private void HandleEntityCreated(ref Entity entity)
        {
            if (_entityCondition(ref entity))
            {
                AddEntity(ref entity);
            }
        }

        private void HandleEntityRemoved(ref Entity entity)
        {
            var index = _ids.IndexOf(entity.Id);
            if (index != -1)
            {
                RemoveEntity(index, ref entity);
            }
        }

        private void HandleComponentCreated(ref Entity entity, ComponentKey key)
        {
            EntityUpdatedHandler(ref entity);
        }

        private void HandleComponentRemoved(ref Entity entity, ComponentKey key)
        {
            EntityUpdatedHandler(ref entity);
        }

        private readonly List<EntityGroupEnumerator> _activeEnumerators = new List<EntityGroupEnumerator>();
        private readonly Action<EntityGroupEnumerator> _handleActiveEnumeratorDisposed;

        public IEnumerator<int> GetEnumerator()
        {
            var enumerator = EntityGroupEnumerator.GetEnumerator(this, _handleActiveEnumeratorDisposed);
            return enumerator;
        }

        private void HandleActiveEnumeratorDisposed(EntityGroupEnumerator entityGroupEnumerator)
        {
            _activeEnumerators.Remove(entityGroupEnumerator);
        }

        private void AddEntity(ref Entity entity)
        {
            _ids.Add(entity.Id);
            EntityAdded?.Invoke(ref entity);
        }

        private void RemoveEntity(int index, ref Entity entity)
        {
            foreach (var entityGroupEnumerator in _activeEnumerators)
            {
                entityGroupEnumerator.HandleItemRemoved(index);
            }

            _ids.RemoveAt(index);
            EntityRemoved?.Invoke(ref entity);
        }

        public class EntityGroupEnumerator : IEnumerator<int>, IDisposable
        {
            private static IObjectPool<EntityGroupEnumerator> _enumeratorsPool =
                new ObjectPool<EntityGroupEnumerator>(() => new EntityGroupEnumerator(),
                    actionOnDestroy: entityGroupEnumerator => entityGroupEnumerator.Dispose());

            private EntityGroup _entityGroup;
            private Action<EntityGroupEnumerator> _disposed;
            private int _enumeratorIndex;

            private EntityGroupEnumerator()
            {
            }

            public static EntityGroupEnumerator GetEnumerator(EntityGroup group, Action<EntityGroupEnumerator> disposed)
            {
                var enumerator = _enumeratorsPool.Get();
                enumerator._entityGroup = group;
                enumerator._disposed = disposed;
                return enumerator;
            }

            public bool MoveNext()
            {
                if (++_enumeratorIndex >= _entityGroup._ids.Count)
                {
                    return false;
                }

                return true;
            }

            public void Reset()
            {
                _enumeratorIndex = -1;
            }

            public int Current => _entityGroup._ids[_enumeratorIndex];

            object IEnumerator.Current => Current;

            public void HandleItemRemoved(int index)
            {
                if (index >= _enumeratorIndex)
                {
                    _enumeratorIndex--;
                }
            }

            public void Dispose()
            {
                Reset();
                _enumeratorsPool.Release(this);
                _disposed?.Invoke(this);
                _disposed = null;
            }
        }
    }
}
