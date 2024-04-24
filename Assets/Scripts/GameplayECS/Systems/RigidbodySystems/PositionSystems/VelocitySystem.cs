using System;
using Asteroids.GameplayECS.Components;
using Asteroids.Services;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;

namespace Asteroids.GameplayECS.Systems.PositionSystems
{
    public class VelocitySystem : IExecutableSystem, IDisposable
    {
        private readonly IFrameInfoService _frameInfoService;
        private readonly ValueTypeECS.EntityContainer.World _world;

        private EntityGroup _entityGroup;

        public VelocitySystem(IInstanceSpawner instanceSpawner, IFrameInfoService frameInfoService, ValueTypeECS.EntityContainer.World world)
        {
            _frameInfoService = frameInfoService;
            _world = world;
            _entityGroup = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<PositionComponent>()
                .RequireComponent<VelocityComponent>()
                .Build();
        }

        public void Dispose()
        {
            _entityGroup.Dispose();
            _entityGroup = null;
        }

        public void Execute()
        {
            var deltaTime = _frameInfoService.DeltaTime;
            foreach (var entityId in _entityGroup)
            {
                ref var entity = ref _world.GetEntity(entityId);
                ref var positionComponent = ref entity.GetComponent<PositionComponent>();
                ref var velocityComponent = ref entity.GetComponent<VelocityComponent>();
                positionComponent.Position += velocityComponent.Velocity * deltaTime;
            }
        }
    }
}
