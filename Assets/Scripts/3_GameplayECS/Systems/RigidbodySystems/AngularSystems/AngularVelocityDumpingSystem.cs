using System;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;

namespace Asteroids.GameplayECS.Systems.AngularSystems
{
    public class AngularVelocityDumpingSystem : AbstractExecutableSystem
    {
        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<UpdatableAngularForceComponent>()
               .RequireComponent<AngularVelocityComponent>()
               .RequireComponent<AngularVelocityDumpComponent>()
               .Build();
        }

        public override void Execute()
        {
            EntityGroup.ForEachComponents<UpdatableAngularForceComponent, AngularVelocityComponent, AngularVelocityDumpComponent>(Execute);
        }

        private static void Execute(ref Entity entity, ref UpdatableAngularForceComponent forceComponent, ref AngularVelocityComponent angularVelocityComponent, ref AngularVelocityDumpComponent angularVelocityDumpComponent)
        {
            var speed = angularVelocityComponent.AngularSpeed;
            var speedSquare = (float)Math.Pow(speed * angularVelocityDumpComponent.StartFactor, 2);
            var sign = speed > 0 ? 1 : -1;
            forceComponent.AngularForce = forceComponent.AngularForce - sign * speedSquare * angularVelocityDumpComponent.TotalFactor;
        }
    }
}
