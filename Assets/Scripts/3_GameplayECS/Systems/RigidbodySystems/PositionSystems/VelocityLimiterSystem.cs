using System;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;
using UnityEngine;

namespace Asteroids.GameplayECS.Systems.PositionSystems
{
    public class VelocityLimiterSystem : IExecutableSystem, IDisposable
    {
        private EntityGroup EntityGroup;

        public VelocityLimiterSystem(IInstanceSpawner instanceSpawner)
        {
            EntityGroup = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<VelocityComponent>()
                .RequireComponent<VelocityLimiterComponent>()
                .Build();
        }

        public void Dispose()
        {
            EntityGroup.Dispose();
            EntityGroup = null;
        }

        void IExecutableSystem.Execute()
        {
            EntityGroup.ForEachOnlyComponents<VelocityComponent, VelocityLimiterComponent>(Execute);
        }

        private static void Execute(ref VelocityComponent velocityComponent, ref VelocityLimiterComponent velocityLimiterComponent)
        {
            velocityComponent.Velocity = Vector2.ClampMagnitude(velocityComponent.Velocity, velocityLimiterComponent.MaxSpeed);
        }
    }
}
