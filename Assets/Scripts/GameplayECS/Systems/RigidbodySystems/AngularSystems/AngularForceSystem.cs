using System;
using Asteroids.GameplayECS.Components;
using Asteroids.Services;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Entities;
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
            float deltaTime = _frameInfoService.DeltaTime;
            foreach (int entityId in EntityGroup)
            {
                ref Entity entity = ref _world.GetEntity(entityId);
                ref UpdatableAngularForceComponent forceComponent = ref entity.GetComponent<UpdatableAngularForceComponent>();
                if (!forceComponent.IsApplied)
                {
                    return;
                }

                ref MassComponent massComponent = ref entity.GetComponent<MassComponent>();
                ref AngularVelocityComponent velocityComponent = ref entity.GetComponent<AngularVelocityComponent>();
                velocityComponent.AngularSpeed += forceComponent.AngularForce / massComponent.Mass * deltaTime;
            }
        }
    }
}
