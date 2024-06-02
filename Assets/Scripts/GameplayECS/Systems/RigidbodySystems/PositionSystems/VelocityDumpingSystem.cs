using System;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;
using UnityEngine;

namespace Asteroids.GameplayECS.Systems.PositionSystems
{
    public class VelocityDumpingSystem : IExecutableSystem, IDisposable
    {
        private readonly ActionReference<Entity, UpdatableForceComponent, VelocityComponent, VelocityDumpComponent> _executeActionReference = Execute;

        private EntityGroup EntityGroup;

        public VelocityDumpingSystem(IInstanceSpawner instanceSpawner)
        {
            EntityGroup = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<UpdatableForceComponent>()
                .RequireComponent<VelocityComponent>()
                .RequireComponent<VelocityDumpComponent>()
                .Build();
        }

        public void Dispose()
        {
            EntityGroup.Dispose();
            EntityGroup = null;
        }

        void IExecutableSystem.Execute()
        {
            EntityGroup.ForEachComponents<UpdatableForceComponent, VelocityComponent, VelocityDumpComponent>(_executeActionReference);
        }

        private static void Execute(ref Entity entity, ref UpdatableForceComponent forceComponent, ref VelocityComponent velocityComponent, ref VelocityDumpComponent velocityDumpComponent)
        {
            Vector2 velocity = velocityComponent.Velocity;
            float velocitySquareMagnitude = (float)Math.Pow(velocity.magnitude * velocityDumpComponent.StartFactor, 2);
            forceComponent.Force = forceComponent.Force - velocity.normalized * velocitySquareMagnitude * velocityDumpComponent.TotalFactor;
        }
    }
}
