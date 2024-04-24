using System;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Factories;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;

namespace Asteroids.GameplayECS.Systems.Laser
{
    public class LaserCollisionHandlingSystem : ISystem, IDisposable
    {
        private readonly EntityFactory _entityFactory;
        private readonly ValueTypeECS.EntityContainer.World _world;

        private EntityGroup EntityGroup;

        public LaserCollisionHandlingSystem(EntityFactory entityFactory, ValueTypeECS.EntityContainer.World world,
            IInstanceSpawner instanceSpawner)
        {
            _entityFactory = entityFactory;
            _world = world;

            EntityGroup = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<LaserComponent>()
                .RequireComponent<CollisionComponent>()
                .Build();

            EntityGroup.EntityAdded += CollisionAddedHandler;
        }

        public void Dispose()
        {
            EntityGroup.EntityAdded -= CollisionAddedHandler;
            EntityGroup.Dispose();
            EntityGroup = null;
        }

        private void CollisionAddedHandler(ref Entity entity)
        {
            var collisionComponent = entity.GetComponent<CollisionComponent>();
            ref var collidedEntity = ref _world.GetEntity(collisionComponent.EntityId);
            if (collidedEntity.HasComponent<RewardableScoreComponent>())
            {
                ref var rewardableScoreComponent = ref collidedEntity.GetComponent<RewardableScoreComponent>();
                _entityFactory.CreateRewardedScoreEntity(rewardableScoreComponent.Score);
            }

            entity.RemoveComponent<CollisionComponent>();
            collidedEntity.CreateComponent(new ReceivedDamageComponent { SourceEntityId = entity.Id });
        }
    }
}
