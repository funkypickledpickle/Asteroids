using Asteroids.GameplayECS.Components;
using Asteroids.Services.Project;
using Asteroids.ValueTypeECS.EntityGroup;
using Zenject;

namespace Asteroids.GameplayECS.Systems.AngularSystems
{
    public class AngularForceSystem : AbstractExecutableSystem
    {
        [Inject] private readonly IFrameInfoService _frameInfoService;

        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<UpdatableAngularForceComponent>()
               .RequireComponent<MassComponent>()
               .RequireComponent<AngularVelocityComponent>()
               .Build();
        }

        public override void Execute()
        {
            var deltaTime = _frameInfoService.DeltaTime;
            foreach (var entityId in EntityGroup)
            {
                ref var entity = ref World.GetEntity(entityId);
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
