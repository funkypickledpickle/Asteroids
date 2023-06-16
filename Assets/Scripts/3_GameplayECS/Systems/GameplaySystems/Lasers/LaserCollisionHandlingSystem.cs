using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Factories;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Zenject;

namespace Asteroids.GameplayECS.Systems.Laser
{
    public class LaserCollisionHandlingSystem : AbstractSystem
    {
        [Inject] private readonly EntityFactory _entityFactory;

        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<LaserComponent>()
               .RequireComponent<CollisionComponent>()
               .Build();
        }

        protected override void InitializeInternal()
        {
            EntityGroup.SubscribeToEntityAddedEvent(CollisionAddedHandler);
        }

        private void CollisionAddedHandler(ref Entity entity)
        {
            var collisionComponent = entity.GetComponent<CollisionComponent>();
            ref var collidedEntity = ref World.GetEntity(collisionComponent.EntityId);
            entity.RemoveComponent<CollisionComponent>();
            collidedEntity.CreateComponent(new ReceivedDamageComponent { SourceEntityId = entity.Id });
        }
    }
}
