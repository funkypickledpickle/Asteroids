using System;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Delegates;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;
using UnityEngine;

namespace Asteroids.GameplayECS.Systems.Engine
{
    public class MainEngineRunningSystem : IExecutableSystem, IDisposable
    {
        private readonly ActionReference<Entity> _executeActionReference = Execute;

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
            EntityGroup.ForEach(_executeActionReference);
        }

        private static void Execute(ref Entity entity)
        {
            ref MainEngineConfigurationComponent engineConfigurationComponent = ref entity.GetComponent<MainEngineConfigurationComponent>();
            ref MainEngineComponent engineComponent = ref entity.GetComponent<MainEngineComponent>();
            if (!engineComponent.IsActive)
            {
                return;
            }

            ref RotationComponent rotationComponent = ref entity.GetComponent<RotationComponent>();
            ref UpdatableForceComponent forceComponent = ref entity.GetComponent<UpdatableForceComponent>();
            float rotationDegrees = entity.HasComponent<MainEngineMagicRotationComponent>() ? entity.GetComponent<MainEngineMagicRotationComponent>().Rotation : rotationComponent.RotationDegrees;

            Vector3 eulerAngles = Vector3.forward * rotationDegrees;
            Quaternion rotation = Quaternion.Euler(eulerAngles);
            Vector3 direction = (rotation * Vector3.up).normalized;
            forceComponent.Force += (Vector2)(direction * engineConfigurationComponent.MaxForce * engineComponent.Acceleration);
        }
    }
}
