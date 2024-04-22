using System;
using Asteroids.GameplayECS.Components;
using Asteroids.Services;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;

namespace Asteroids.GameplayECS.Systems.PositionSystems
{
    public class ForceSystem : IExecutableSystem, IDisposable
    {
        private readonly IFrameInfoService _frameInfoService;
        private readonly ValueTypeECS.EntityContainer.World _world;

        private EntityGroup EntityGroup;

        public ForceSystem(IFrameInfoService frameInfoService, ValueTypeECS.EntityContainer.World world, IInstanceSpawner instanceSpawner)
        {
            _frameInfoService = frameInfoService;
            _world = world;

            EntityGroup = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<UpdatableForceComponent>()
                .RequireComponent<MassComponent>()
                .RequireComponent<VelocityComponent>()
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
                ref var forceComponent = ref entity.GetComponent<UpdatableForceComponent>();
                if (!forceComponent.IsApplied)
                {
                    continue;
                }

                ref var massComponent = ref entity.GetComponent<MassComponent>();
                ref var velocityComponent = ref entity.GetComponent<VelocityComponent>();
                velocityComponent.Velocity += forceComponent.Force / massComponent.Mass * deltaTime;
            }
        }
    }
}
