using Asteroids.GameplayECS.Components;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;

namespace Asteroids.GameplayECS.Systems.Ship
{
    public class ShipCollisionHandlingSystem : AbstractSystem
    {
        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<ShipComponent>()
               .RequireComponent<CollisionComponent>()
               .Build();
        }

        protected override void InitializeInternal()
        {
            EntityGroup.SubscribeToEntityAddedEvent(CollisionAddedHandler);
        }

        private void CollisionAddedHandler(ref Entity entity)
        {
            ref var collisionComponent = ref entity.GetComponent<CollisionComponent>();
            ref var collidedEntity = ref World.GetEntity(collisionComponent.EntityId);
            if (collidedEntity.HasComponent<UFOComponent>() || collidedEntity.HasComponent<AsteroidComponent>())
            {
                entity.CreateComponent(new ReceivedDamageComponent { SourceEntityId = collidedEntity.Id });
            }

            entity.RemoveComponent<CollisionComponent>();
            collidedEntity.CreateComponent(new ReceivedDamageComponent { SourceEntityId = entity.Id });
        }
    }
}
