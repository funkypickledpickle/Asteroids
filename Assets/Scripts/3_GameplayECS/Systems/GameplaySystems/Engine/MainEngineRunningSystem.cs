using System;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;
using UnityEngine;

namespace Asteroids.GameplayECS.Systems.Engine
{
    public class MainEngineRunningSystem : IExecutableSystem, IDisposable
    {
        private EntityGroup EntityGroup;

        public MainEngineRunningSystem(IInstanceSpawner instanceSpawner)
        {
            EntityGroup = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<MainEngineConfigurationComponent>()
                .RequireComponent<MainEngineComponent>()
                .RequireComponent<RotationComponent>()
                .RequireComponent<UpdatableForceComponent>()
                .Build();
        }

        public void Dispose()
        {
            EntityGroup.Dispose();
            EntityGroup = null;
        }

        void IExecutableSystem.Execute()
        {
            EntityGroup.ForEach(Execute);
        }

        private static void Execute(ref Entity entity)
        {
            ref var engineConfigurationComponent = ref entity.GetComponent<MainEngineConfigurationComponent>();
            ref var engineComponent = ref entity.GetComponent<MainEngineComponent>();
            if (!engineComponent.IsActive)
            {
                return;
            }

            ref var rotationComponent = ref entity.GetComponent<RotationComponent>();
            ref var forceComponent = ref entity.GetComponent<UpdatableForceComponent>();
            var rotationDegrees = entity.HasComponent<MainEngineMagicRotationComponent>() ? entity.GetComponent<MainEngineMagicRotationComponent>().Rotation : rotationComponent.RotationDegrees;

            var eulerAngles = Vector3.forward * rotationDegrees;
            var rotation = Quaternion.Euler(eulerAngles);
            var direction = (rotation * Vector3.up).normalized;
            forceComponent.Force += (Vector2)(direction * engineConfigurationComponent.MaxForce * engineComponent.Acceleration);
        }
    }
}
