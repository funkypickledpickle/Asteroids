using System;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Delegates;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;

namespace Asteroids.GameplayECS.Systems.Engine
{
    public class RotationEngineRunningSystem : IExecutableSystem, IDisposable
    {
        private readonly ActionReference<Entity> _executeActionReference = Execute;

        private EntityGroup _ships;

        public RotationEngineRunningSystem(IInstanceSpawner instanceSpawner)
        {
            _ships = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<RotationEngineConfigurationComponent>()
                .RequireComponent<RotationEngineComponent>()
                .RequireComponent<UpdatableAngularForceComponent>()
                .Build();
        }

        public void Dispose()
        {
            _ships.Dispose();
            _ships = null;
        }

        void IExecutableSystem.Execute()
        {
            _ships.ForEach(_executeActionReference);
        }

        private static void Execute(ref Entity entity)
        {
            ref RotationEngineConfigurationComponent rotationEngineConfigurationComponent = ref entity.GetComponent<RotationEngineConfigurationComponent>();
            ref RotationEngineComponent rotationEngineComponent = ref entity.GetComponent<RotationEngineComponent>();
            if (!rotationEngineComponent.IsActive)
            {
                return;
            }

            ref UpdatableAngularForceComponent angularForceComponent = ref entity.GetComponent<UpdatableAngularForceComponent>();
            angularForceComponent.AngularForce += rotationEngineComponent.Rotation * rotationEngineConfigurationComponent.MaxAngularForce;
        }
    }
}
