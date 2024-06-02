using System.Collections.Generic;
using System.Linq;
using Asteroids.ValueTypeECS.Components;
using Asteroids.ValueTypeECS.Delegates;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityContainer;
using Zenject;

namespace Asteroids.ValueTypeECS.EntityGroup
{
    public class EntityGroupBuilder
    {
        [Inject] private readonly World _world;

        private readonly List<FunctionReference<Entity, bool>> _conditions = new List<FunctionReference<Entity, bool>>();

        public EntityGroupBuilder RequireComponent<TComponent>() where TComponent : struct, IECSComponent
        {
            _conditions.Add((ref Entity referenced) => referenced.HasComponent<TComponent>());
            return this;
        }

        public EntityGroupBuilder RequireComponentAbsence<TComponent>() where TComponent : struct, IECSComponent
        {
            _conditions.Add((ref Entity referenced) => !referenced.HasComponent<TComponent>());
            return this;
        }

        public EntityGroup Build()
        {
            return new EntityGroup(_world, new Matcher(_conditions.ToList()).CheckEntity);
        }

        private class Matcher
        {
            private readonly List<FunctionReference<Entity, bool>> _conditions;

            public Matcher(List<FunctionReference<Entity, bool>> conditions)
            {
                _conditions = conditions;
            }

            public bool CheckEntity(ref Entity entity)
            {
                for (int i = 0; i < _conditions.Count; i++)
                {
                    if (!_conditions[i](ref entity))
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
