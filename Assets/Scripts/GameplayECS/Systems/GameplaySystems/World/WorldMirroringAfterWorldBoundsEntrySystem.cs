using System;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;

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
                ref var bounds = ref _bounds.GetFirst().GetComponent<WorldBoundsComponent>().Bounds;

                foreach (var entityId in _entitiesOutsideBounds)
                {
                    ref var entity = ref _world.GetEntity(entityId);
                    ref var positionComponent = ref entity.GetComponent<PositionComponent>();
                    ref var position = ref positionComponent.Position;

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
