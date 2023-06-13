using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;

namespace Asteroids.GameplayECS.Systems.Engine
{
    public class RotationEngineRunningSystem : AbstractExecutableSystem
    {
        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<RotationEngineConfigurationComponent>()
               .RequireComponent<RotationEngineComponent>()
               .RequireComponent<UpdatableAngularForceComponent>()
               .Build();
        }

        public override void Execute()
        {
            EntityGroup.ForEach(Execute);
        }

        private static void Execute(ref Entity entity)
        {
            ref var rotationEngineConfigurationComponent = ref entity.GetComponent<RotationEngineConfigurationComponent>();
            ref var rotationEngineComponent = ref entity.GetComponent<RotationEngineComponent>();
            if (!rotationEngineComponent.IsActive)
            {
                return;
            }

            ref var angularForceComponent = ref entity.GetComponent<UpdatableAngularForceComponent>();
            angularForceComponent.AngularForce += rotationEngineComponent.Rotation * rotationEngineConfigurationComponent.MaxAngularForce;
        }
    }
}
