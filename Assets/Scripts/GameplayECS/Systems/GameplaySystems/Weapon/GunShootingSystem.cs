using System;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.GameplayECS.Factories;
using Asteroids.Services;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Delegates;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;
using UnityEngine;
using Zenject;

namespace Asteroids.GameplayECS.Systems.Weapon
{
    public class GunShootingSystem : IExecutableSystem, IDisposable
    {
        private readonly ActionReferenceValue<Entity, float> _executeAction;

        private readonly IFrameInfoService _frameInfoService;
        private readonly EntityFactory _entityFactory;

        private EntityGroup _armedEntities;

        public GunShootingSystem(IFrameInfoService frameInfoService, EntityFactory entityFactory, IInstanceSpawner instanceSpawner)
        {
            _frameInfoService = frameInfoService;
            _entityFactory = entityFactory;

            _armedEntities = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<RotationComponent>()
                .RequireComponent<GunComponent>()
                .RequireComponent<VelocityComponent>()
                .Build();

            _executeAction = Execute;
        }

        public void Dispose()
        {
            _armedEntities.Dispose();
            _armedEntities = null;
        }

        void IExecutableSystem.Execute()
        {
            _armedEntities.ForEach(_executeAction, _frameInfoService.StartTime);
        }

        private void Execute(ref Entity entity, float currentTime)
        {
            ref GunComponent gunComponent = ref entity.GetComponent<GunComponent>();
            ref GunConfigurationComponent configuration = ref gunComponent.Configuration;

            if (gunComponent.IsFireRequested && gunComponent.LastFireTime + configuration.FiringInterval < currentTime)
            {
                ref PositionComponent positionComponent = ref entity.GetComponent<PositionComponent>();
                ref RotationComponent rotationComponent = ref entity.GetComponent<RotationComponent>();
                ref VelocityComponent velocityComponent = ref entity.GetComponent<VelocityComponent>();

                gunComponent.LastFireTime = currentTime;

                Vector3 eulerAngles = Vector3.forward * rotationComponent.RotationDegrees;
                Quaternion rotation = Quaternion.Euler(eulerAngles);
                Vector3 direction = (rotation * Vector3.up).normalized;
                Vector2 bulletPosition = positionComponent.Position + (Vector2)(rotation * configuration.BulletSpawnPositionOffset);
                Vector2 bulletVelocity = (Vector2)(direction * configuration.BulletSpeed) + velocityComponent.Velocity;
                _entityFactory.CreateBullet(bulletPosition, rotationComponent.RotationDegrees, bulletVelocity);
            }
        }
    }
}
