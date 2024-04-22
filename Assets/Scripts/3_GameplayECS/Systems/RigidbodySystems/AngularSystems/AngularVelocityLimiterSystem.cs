using System;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;

namespace Asteroids.GameplayECS.Systems.AngularSystems
{
    public class AngularVelocityLimiterSystem : IExecutableSystem, IDisposable
    {
        private EntityGroup EntityGroup;

        public AngularVelocityLimiterSystem(IInstanceSpawner instanceSpawner)
        {
            EntityGroup = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<AngularVelocityComponent>()
                .RequireComponent<AngularVelocityLimiterComponent>()
                .Build();
        }

        public void Dispose()
        {
            EntityGroup.Dispose();
            EntityGroup = null;
        }

        void IExecutableSystem.Execute()
        {
            EntityGroup.ForEachOnlyComponents<AngularVelocityComponent, AngularVelocityLimiterComponent>(Execute);
        }

        private static void Execute(ref AngularVelocityComponent angularVelocityComponent, ref AngularVelocityLimiterComponent angularVelocityLimiterComponent)
        {
            angularVelocityComponent.AngularSpeed = Math.Clamp(angularVelocityComponent.AngularSpeed, -angularVelocityLimiterComponent.MaxSpeed, angularVelocityLimiterComponent.MaxSpeed);
        }
    }
}
