using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.GameplayECS.Factories;
using Asteroids.Services.Project;
using Asteroids.ValueTypeECS.Delegates;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using UnityEngine;
using Zenject;

namespace Asteroids.GameplayECS.Systems.Weapon
{
    public class GunShootingSystem : AbstractExecutableSystem
    {
        [Inject] private readonly IFrameInfoService _frameInfoService;
        [Inject] private readonly EntityFactory _entityFactory;

        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<RotationComponent>()
               .RequireComponent<GunComponent>()
               .RequireComponent<VelocityComponent>()
               .Build();
        }

        public override void Execute()
        {
            EntityGroup.ForEach<float>(Execute, _frameInfoService.StartTime);
        }

        private void Execute(ref Entity entity, float currentTime)
        {
            ref var gunComponent = ref entity.GetComponent<GunComponent>();
            ref var configuration = ref gunComponent.Configuration;

            if (gunComponent.IsFireRequested && gunComponent.LastFireTime + configuration.FiringInterval < currentTime)
            {
                ref var positionComponent = ref entity.GetComponent<PositionComponent>();
                ref var rotationComponent = ref entity.GetComponent<RotationComponent>();
                ref var velocityComponent = ref entity.GetComponent<VelocityComponent>();

                gunComponent.LastFireTime = currentTime;

                var eulerAngles = Vector3.forward * rotationComponent.RotationDegrees;
                var rotation = Quaternion.Euler(eulerAngles);
                var direction = (rotation * Vector3.up).normalized;
                Vector2 bulletPosition = positionComponent.Position + (Vector2)(rotation * configuration.BulletSpawnPositionOffset);
                var bulletVelocity = (Vector2)(direction * configuration.BulletSpeed) + velocityComponent.Velocity;
                _entityFactory.CreateBullet(bulletPosition, rotationComponent.RotationDegrees, bulletVelocity);
            }
        }
    }
}
