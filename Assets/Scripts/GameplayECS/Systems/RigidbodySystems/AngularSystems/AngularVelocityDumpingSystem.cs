using System;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;

namespace Asteroids.GameplayECS.Systems.AngularSystems
{
    public class AngularVelocityDumpingSystem : IExecutableSystem, IDisposable
    {
        private readonly ActionReference<Entity, UpdatableAngularForceComponent, AngularVelocityComponent, AngularVelocityDumpComponent> _executeActionReference = Execute;

        private EntityGroup EntityGroup;

        public AngularVelocityDumpingSystem(IInstanceSpawner instanceSpawner)
        {
            EntityGroup = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<UpdatableAngularForceComponent>()
                .RequireComponent<AngularVelocityComponent>()
                .RequireComponent<AngularVelocityDumpComponent>()
                .Build();
        }

        public void Dispose()
        {
            EntityGroup.Dispose();
            EntityGroup = null;
        }

        void IExecutableSystem.Execute()
        {
            EntityGroup.ForEachComponents(_executeActionReference);
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
