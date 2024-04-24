using System;
using Asteroids.GameplayECS.Components;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;

namespace Asteroids.GameplayECS.Systems.Ship
{
    public class ShipCollisionHandlingSystem : ISystem, IDisposable
    {
        private readonly ValueTypeECS.EntityContainer.World _world;

        private EntityGroup _crashedShips;

        public ShipCollisionHandlingSystem(ValueTypeECS.EntityContainer.World world, IInstanceSpawner instanceSpawner)
        {
            _world = world;

            _crashedShips = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<ShipComponent>()
                .RequireComponent<CollisionComponent>()
                .Build();

            _crashedShips.EntityAdded += CollisionAddedHandler;
        }

        public void Dispose()
        {
            _crashedShips.Dispose();
            _crashedShips = null;
        }

        private void CollisionAddedHandler(ref Entity entity)
        {
            ref var collisionComponent = ref entity.GetComponent<CollisionComponent>();
            ref var collidedEntity = ref _world.GetEntity(collisionComponent.EntityId);
            if (collidedEntity.HasComponent<UFOComponent>() || collidedEntity.HasComponent<AsteroidComponent>())
            {
                entity.CreateComponent(new ReceivedDamageComponent { SourceEntityId = collidedEntity.Id });
            }

            entity.RemoveComponent<CollisionComponent>();
            collidedEntity.CreateComponent(new ReceivedDamageComponent { SourceEntityId = entity.Id });
        }
    }
}
