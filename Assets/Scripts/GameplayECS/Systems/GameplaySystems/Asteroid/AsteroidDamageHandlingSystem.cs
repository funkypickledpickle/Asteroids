using System;
using Asteroids.GameplayECS.Components;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;

namespace Asteroids.GameplayECS.Systems.Asteroid
{
    public class AsteroidDamageHandlingSystem : ISystem, IDisposable
    {
        private readonly ValueTypeECS.EntityContainer.World _world;

        private EntityGroup _entityGroup;

        public AsteroidDamageHandlingSystem(IInstanceSpawner instanceSpawner, ValueTypeECS.EntityContainer.World world)
        {
            _world = world;
            _entityGroup = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<AsteroidComponent>()
                .RequireComponent<ReceivedDamageComponent>()
                .Build();
            _entityGroup.EntityAdded += AsteroidDamagedHandler;
        }

        public void Dispose()
        {
            _entityGroup.EntityAdded -= AsteroidDamagedHandler;
            _entityGroup.Dispose();
            _entityGroup = null;
        }

        private void AsteroidDamagedHandler(ref Entity entity)
        {
            entity.CreateComponent<DestroyedComponent>();
            ref var damageCreator = ref _world.GetEntity(entity.GetComponent<ReceivedDamageComponent>().SourceEntityId);
            if (!damageCreator.HasComponent<LaserComponent>())
            {
                entity.CreateComponent<AsteroidSplitComponent>();
            }
        }
    }
}
