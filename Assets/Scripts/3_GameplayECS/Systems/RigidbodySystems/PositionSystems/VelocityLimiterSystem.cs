using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.ValueTypeECS.EntityGroup;
using UnityEngine;

namespace Asteroids.GameplayECS.Systems.PositionSystems
{
    public class VelocityLimiterSystem : AbstractExecutableSystem
    {
        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<VelocityComponent>()
               .RequireComponent<VelocityLimiterComponent>()
               .Build();
        }

        public override void Execute()
        {
            EntityGroup.ForEachOnlyComponents<VelocityComponent, VelocityLimiterComponent>(Execute);
        }

        private static void Execute(ref VelocityComponent velocityComponent, ref VelocityLimiterComponent velocityLimiterComponent)
        {
            velocityComponent.Velocity = Vector2.ClampMagnitude(velocityComponent.Velocity, velocityLimiterComponent.MaxSpeed);
        }
    }
}
