using System;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.Services;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;

namespace Asteroids.GameplayECS.Systems.Weapon
{
    public class LaserGunAutoChargingSystem : IExecutableSystem, IDisposable
    {
        private readonly ActionReference<LaserGunComponent, LaserAutoChargingComponent> _executeActionReference;

        private readonly IFrameInfoService _frameInfoService;

        private EntityGroup EntityGroup;

        public LaserGunAutoChargingSystem(IFrameInfoService frameInfoService, IInstanceSpawner instanceSpawner)
        {
            _executeActionReference = Execute;

            _frameInfoService = frameInfoService;

            EntityGroup = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<LaserGunComponent>()
                .RequireComponent<LaserAutoChargingComponent>()
                .Build();
        }

        public void Dispose()
        {
            EntityGroup.Dispose();
            EntityGroup = null;
        }

        void IExecutableSystem.Execute()
        {
            EntityGroup.ForEachOnlyComponents(_executeActionReference);
        }

        private void Execute(ref LaserGunComponent laserGunComponent, ref LaserAutoChargingComponent laserAutoChargingComponent)
        {
            if (laserGunComponent.ChargesCount < laserAutoChargingComponent.Configuration.MaxChargesQuantity &&
                laserAutoChargingComponent.LastLoadingTime + laserAutoChargingComponent.Configuration.Duration < _frameInfoService.StartTime)
            {
                if (laserAutoChargingComponent.IsAutoCharging)
                {
                    laserGunComponent.ChargesCount++;
                    laserAutoChargingComponent.IsAutoCharging = false;
                }
                else
                {
                    laserAutoChargingComponent.LastLoadingTime = _frameInfoService.StartTime;
                    laserAutoChargingComponent.IsAutoCharging = true;
                }
            }
        }
    }
}
