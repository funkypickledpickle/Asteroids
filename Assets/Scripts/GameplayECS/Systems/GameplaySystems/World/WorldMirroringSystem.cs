using System;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;
using UnityEngine;

namespace Asteroids.GameplayECS.Systems.World
{
    public class WorldMirroringSystem : IExecutableSystem, IDisposable
    {
        private readonly ValueTypeECS.EntityContainer.World _world;

        private EntityGroup _entities;

        private EntityGroup _bounds;

        public WorldMirroringSystem(ValueTypeECS.EntityContainer.World world, IInstanceSpawner instanceSpawner)
        {
            _world = world;

            _entities = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<PositionComponent>()
                .RequireComponent<VelocityComponent>()
                .RequireComponent<WorldMirroringComponent>()
                .Build();

            _bounds = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<WorldBoundsComponent>()
                .Build();
        }

        public void Dispose()
        {
            _entities.Dispose();
            _entities = null;

            _bounds?.Dispose();
            _bounds = null;
        }

        void IExecutableSystem.Execute()
        {
            if (_bounds.Count != 0)
            {
                ref Rect bounds = ref _bounds.GetFirst().GetComponent<WorldBoundsComponent>().Bounds;

                foreach (int entityId in _entities)
                {
                    ref Entity entity = ref _world.GetEntity(entityId);
                    ref PositionComponent positionComponent = ref entity.GetComponent<PositionComponent>();
                    ref Vector2 position = ref positionComponent.Position;
                    if (position.x < bounds.xMin)
                    {
                        position.x += bounds.size.x;
                    }
                    else if (position.x > bounds.xMax)
                    {
                        position.x -= bounds.size.x;
                    }

                    if (position.y < bounds.yMin)
                    {
                        position.y += bounds.size.y;
                    }
                    else if (position.y > bounds.yMax)
                    {
                        position.y -= bounds.size.y;
                    }
                }
            }
        }
    }
}
