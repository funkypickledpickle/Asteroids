using System;
using Asteroids.GameplayECS.Components;
using Asteroids.Services;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;

namespace Asteroids.GameplayECS.Systems.AngularSystems
{
    public class AngularForceSystem : IExecutableSystem, IDisposable
    {
        private readonly IFrameInfoService _frameInfoService;
        private readonly ValueTypeECS.EntityContainer.World _world;

        private EntityGroup EntityGroup;

        public AngularForceSystem(IFrameInfoService frameInfoService, ValueTypeECS.EntityContainer.World world, IInstanceSpawner instanceSpawner)
        {
            _frameInfoService = frameInfoService;
            _world = world;

            EntityGroup = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<UpdatableAngularForceComponent>()
                .RequireComponent<MassComponent>()
                .RequireComponent<AngularVelocityComponent>()
                .Build();
        }

        public void Dispose()
        {
            EntityGroup.Dispose();
            EntityGroup = null;
        }

        void IExecutableSystem.Execute()
        {
            var deltaTime = _frameInfoService.DeltaTime;
            foreach (var entityId in EntityGroup)
            {
                ref var entity = ref _world.GetEntity(entityId);
                ref var forceComponent = ref entity.GetComponent<UpdatableAngularForceComponent>();
                if (!forceComponent.IsApplied)
                {
                    return;
                }

                ref var massComponent = ref entity.GetComponent<MassComponent>();
                ref var velocityComponent = ref entity.GetComponent<AngularVelocityComponent>();
                velocityComponent.AngularSpeed += forceComponent.AngularForce / massComponent.Mass * deltaTime;
            }
        }
    }
}
