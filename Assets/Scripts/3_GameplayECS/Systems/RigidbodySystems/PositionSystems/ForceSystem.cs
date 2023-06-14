using Asteroids.GameplayECS.Components;
using Asteroids.Services.Project;
using Asteroids.ValueTypeECS.EntityGroup;
using Zenject;

namespace Asteroids.GameplayECS.Systems.PositionSystems
{
    public class ForceSystem : AbstractExecutableSystem
    {
        [Inject] private readonly IFrameInfoService _frameInfoService;

        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<UpdatableForceComponent>()
               .RequireComponent<MassComponent>()
               .RequireComponent<VelocityComponent>()
               .Build();
        }

        public override void Execute()
        {
            var deltaTime = _frameInfoService.DeltaTime;
            foreach (var entityId in EntityGroup)
            {
                ref var entity = ref World.GetEntity(entityId);
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
