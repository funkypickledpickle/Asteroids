using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.Services.Project;
using Asteroids.ValueTypeECS.EntityGroup;
using Zenject;

namespace Asteroids.GameplayECS.Systems.Weapon
{
    public class LaserGunAutoChargingSystem : AbstractExecutableSystem
    {
        [Inject] private readonly IFrameInfoService _frameInfoService;

        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<LaserGunComponent>()
               .RequireComponent<LaserAutoChargingComponent>()
               .Build();
        }

        public override void Execute()
        {
            EntityGroup.ForEachOnlyComponents<LaserGunComponent, LaserAutoChargingComponent>(Execute);
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
