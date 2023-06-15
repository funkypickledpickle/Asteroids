using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using UnityEngine;

namespace Asteroids.GameplayECS.Systems.Engine
{
    public class MainEngineRunningSystem : AbstractExecutableSystem
    {
        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<MainEngineConfigurationComponent>()
               .RequireComponent<MainEngineComponent>()
               .RequireComponent<RotationComponent>()
               .RequireComponent<UpdatableForceComponent>()
               .Build();
        }

        public override void Execute()
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
            var rotationDegrees = rotationComponent.RotationDegrees;

            var eulerAngles = Vector3.forward * rotationDegrees;
            var rotation = Quaternion.Euler(eulerAngles);
            var direction = (rotation * Vector3.up).normalized;
            forceComponent.Force += (Vector2)(direction * engineConfigurationComponent.MaxForce * engineComponent.Acceleration);
        }
    }
}
