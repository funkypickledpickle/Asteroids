using System;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;

namespace Asteroids.GameplayECS.Systems.PositionSystems
{
    public class VelocityDumpingSystem : AbstractExecutableSystem
    {
        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<UpdatableForceComponent>()
               .RequireComponent<VelocityComponent>()
               .RequireComponent<VelocityDumpComponent>()
               .Build();
        }

        public override void Execute()
        {
            EntityGroup.ForEachComponents<UpdatableForceComponent, VelocityComponent, VelocityDumpComponent>(Execute);
        }

        private static void Execute(ref Entity entity, ref UpdatableForceComponent forceComponent, ref VelocityComponent velocityComponent, ref VelocityDumpComponent velocityDumpComponent)
        {
            var velocity = velocityComponent.Velocity;
            var velocitySquareMagnitude = (float)Math.Pow(velocity.magnitude * velocityDumpComponent.StartFactor, 2);
            forceComponent.Force = forceComponent.Force - velocity.normalized * velocitySquareMagnitude * velocityDumpComponent.TotalFactor;
        }
    }
}
