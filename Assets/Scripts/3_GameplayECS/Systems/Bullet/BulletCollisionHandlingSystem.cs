using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Factories;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Zenject;

namespace Asteroids.GameplayECS.Systems.Bullet
{
    public class BulletCollisionHandlingSystem : AbstractSystem
    {
        [Inject] private readonly EntityFactory _entityFactory;

        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<BulletComponent>()
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
            entity.CreateComponent<DestroyedComponent>();
            collidedEntity.CreateComponent(new ReceivedDamageComponent { SourceEntityId = entity.Id });
        }
    }
}
