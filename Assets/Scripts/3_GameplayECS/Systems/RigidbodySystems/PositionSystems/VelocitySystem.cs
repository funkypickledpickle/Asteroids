using Asteroids.GameplayECS.Components;
using Asteroids.Services.Project;
using Asteroids.ValueTypeECS.EntityGroup;
using Zenject;

namespace Asteroids.GameplayECS.Systems.PositionSystems
{
    public class VelocitySystem : AbstractExecutableSystem
    {
        [Inject] private IFrameInfoService _frameInfoService;

        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<PositionComponent>()
               .RequireComponent<VelocityComponent>()
               .Build();
        }

        public override void Execute()
        {
            var deltaTime = _frameInfoService.DeltaTime;
            foreach (var entityId in EntityGroup)
            {
                ref var entity = ref World.GetEntity(entityId);
                ref var positionComponent = ref entity.GetComponent<PositionComponent>();
                ref var velocityComponent = ref entity.GetComponent<VelocityComponent>();
                positionComponent.Position += velocityComponent.Velocity * deltaTime;
            }
        }
    }
}
