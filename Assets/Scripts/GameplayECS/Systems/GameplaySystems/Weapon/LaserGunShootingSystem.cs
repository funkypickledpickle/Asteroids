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

namespace Asteroids.GameplayECS.Systems.Weapon
{
    public class LaserGunShootingSystem : IExecutableSystem, IDisposable
    {
        private readonly IFrameInfoService _frameInfoService;
        private readonly EntityFactory _entityFactory;

        private EntityGroup _laserArmedEntities;

        private readonly ActionReferenceValue<Entity, float> _executeAction;

        public LaserGunShootingSystem(IFrameInfoService frameInfoService, EntityFactory entityFactory, IInstanceSpawner instanceSpawner)
        {
            _executeAction = Execute;

            _frameInfoService = frameInfoService;
            _entityFactory = entityFactory;

            _laserArmedEntities = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<LaserGunComponent>()
                .RequireComponent<RotationComponent>()
                .RequireComponent<VelocityComponent>()
                .Build();
        }

        public void Dispose()
        {
            _laserArmedEntities.Dispose();
            _laserArmedEntities = null;
        }

        void IExecutableSystem.Execute()
        {
            _laserArmedEntities.ForEach(_executeAction, _frameInfoService.StartTime);
        }

        private void Execute(ref Entity entity, float currentTime)
        {
            ref LaserGunComponent gunComponent = ref entity.GetComponent<LaserGunComponent>();
            ref LaserGunConfigurationComponent configuration = ref gunComponent.Configuration;

            if (gunComponent.IsFireRequested && gunComponent.LastFireTime + configuration.FiringInterval < currentTime && gunComponent.ChargesCount != 0)
            {
                gunComponent.LastFireTime = currentTime;
                gunComponent.ChargesCount--;

                _entityFactory.CreateLaser(entity.Id, configuration.LaserSpawnPositionOffset, configuration.Distance);
            }
        }
    }
}
