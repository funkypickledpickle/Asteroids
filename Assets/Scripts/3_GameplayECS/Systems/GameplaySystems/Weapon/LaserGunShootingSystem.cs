using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.GameplayECS.Factories;
using Asteroids.Services.Project;
using Asteroids.ValueTypeECS.Delegates;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Zenject;

namespace Asteroids.GameplayECS.Systems.Weapon
{
    public class LaserGunShootingSystem : AbstractExecutableSystem
    {
        [Inject] private readonly IFrameInfoService _frameInfoService;
        [Inject] private readonly EntityFactory _entityFactory;

        private readonly ActionReferenceValue<Entity, float> _executeAction;

        public LaserGunShootingSystem()
        {
            _executeAction = Execute;
        }

        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<LaserGunComponent>()
               .RequireComponent<RotationComponent>()
               .RequireComponent<VelocityComponent>()
               .Build();
        }

        public override void Execute()
        {
            EntityGroup.ForEach(_executeAction, _frameInfoService.StartTime);
        }

        private void Execute(ref Entity entity, float currentTime)
        {
            ref var gunComponent = ref entity.GetComponent<LaserGunComponent>();
            ref var configuration = ref gunComponent.Configuration;

            if (gunComponent.IsFireRequested && gunComponent.LastFireTime + configuration.FiringInterval < currentTime && gunComponent.ChargesCount != 0)
            {
                gunComponent.LastFireTime = currentTime;
                gunComponent.ChargesCount--;

                _entityFactory.CreateLaser(entity.Id, configuration.LaserSpawnPositionOffset, configuration.Distance);
            }
        }
    }
}
