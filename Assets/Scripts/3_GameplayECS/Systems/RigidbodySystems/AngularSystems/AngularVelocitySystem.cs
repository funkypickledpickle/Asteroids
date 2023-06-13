using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.Services.Project;
using Asteroids.ValueTypeECS.EntityGroup;
using Zenject;

namespace Asteroids.GameplayECS.Systems.AngularSystems
{
    public class AngularVelocitySystem : AbstractExecutableSystem
    {
        [Inject] private readonly IFrameInfoService _frameInfoService;

        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<RotationComponent>()
               .RequireComponent<AngularVelocityComponent>()
               .Build();
        }

        public override void Execute()
        {
            EntityGroup.ForEachOnlyComponents<RotationComponent, AngularVelocityComponent, float>(Execute, _frameInfoService.DeltaTime);
        }

        private static void Execute(ref RotationComponent rotationComponent, ref AngularVelocityComponent angularVelocityComponent, float deltaTime)
        {
            rotationComponent.RotationDegrees += angularVelocityComponent.AngularSpeed * deltaTime;
            if (rotationComponent.RotationDegrees < 0)
            {
                rotationComponent.RotationDegrees += 360;
            }
            else if (rotationComponent.RotationDegrees > 360)
            {
                rotationComponent.RotationDegrees -= 360;
            }
        }
    }
}
