using System;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.ValueTypeECS.EntityGroup;

namespace Asteroids.GameplayECS.Systems.AngularSystems
{
    public class AngularVelocityLimiterSystem : AbstractExecutableSystem
    {
        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<AngularVelocityComponent>()
               .RequireComponent<AngularVelocityLimiterComponent>()
               .Build();
        }

        public override void Execute()
        {
            EntityGroup.ForEachOnlyComponents<AngularVelocityComponent, AngularVelocityLimiterComponent>(Execute);
        }

        private static void Execute(ref AngularVelocityComponent angularVelocityComponent, ref AngularVelocityLimiterComponent angularVelocityLimiterComponent)
        {
            angularVelocityComponent.AngularSpeed = Math.Clamp(angularVelocityComponent.AngularSpeed, -angularVelocityLimiterComponent.MaxSpeed, angularVelocityLimiterComponent.MaxSpeed);
        }
    }
}
