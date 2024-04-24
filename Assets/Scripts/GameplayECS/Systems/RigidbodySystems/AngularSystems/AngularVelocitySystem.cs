using System;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.Services;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;

namespace Asteroids.GameplayECS.Systems.AngularSystems
{
    public class AngularVelocitySystem : IExecutableSystem, IDisposable
    {
        private readonly ActionReferenceValue<RotationComponent, AngularVelocityComponent, float> _executeAction = Execute;

        private readonly IFrameInfoService _frameInfoService;

        private EntityGroup EntityGroup;

        public AngularVelocitySystem(IFrameInfoService frameInfoService, IInstanceSpawner instanceSpawner)
        {
            _frameInfoService = frameInfoService;

            EntityGroup = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<RotationComponent>()
                .RequireComponent<AngularVelocityComponent>()
                .Build();
        }

        public void Dispose()
        {
            EntityGroup.Dispose();
            EntityGroup = null;
        }

        void IExecutableSystem.Execute()
        {
            EntityGroup.ForEachOnlyComponents(_executeAction, _frameInfoService.DeltaTime);
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
