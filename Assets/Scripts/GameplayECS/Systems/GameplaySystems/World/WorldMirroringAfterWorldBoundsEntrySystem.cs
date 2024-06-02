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
    public class WorldMirroringAfterBoundsEntrySystem : IExecutableSystem, IDisposable
    {
        private readonly ValueTypeECS.EntityContainer.World _world;

        private EntityGroup _entitiesOutsideBounds;

        private EntityGroup _bounds;

        public WorldMirroringAfterBoundsEntrySystem(ValueTypeECS.EntityContainer.World world, IInstanceSpawner instanceSpawner)
        {
            _world = world;

            _entitiesOutsideBounds = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<WorldMirroringAfterWorldBoundsEntryComponent>()
                .RequireComponent<PositionComponent>()
                .Build();

            _bounds = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<WorldBoundsComponent>()
                .Build();
        }

        public void Dispose()
        {
            _entitiesOutsideBounds.Dispose();
            _entitiesOutsideBounds = null;

            _bounds.Dispose();
            _bounds = null;
        }

        public void Execute()
        {
            if (_bounds.Count != 0)
            {
                ref Rect bounds = ref _bounds.GetFirst().GetComponent<WorldBoundsComponent>().Bounds;

                foreach (int entityId in _entitiesOutsideBounds)
                {
                    ref Entity entity = ref _world.GetEntity(entityId);
                    ref PositionComponent positionComponent = ref entity.GetComponent<PositionComponent>();
                    ref Vector2 position = ref positionComponent.Position;

                    if (bounds.Contains(position))
                    {
                        entity.RemoveComponent<WorldMirroringAfterWorldBoundsEntryComponent>();
                        entity.CreateComponent<WorldMirroringComponent>();
                    }
                }
            }
        }
    }
}
