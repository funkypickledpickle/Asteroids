using Asteroids.GameplayECS.Components;
using Asteroids.ValueTypeECS.EntityGroup;

namespace Asteroids.GameplayECS.Systems.PositionSystems
{
    public class ForceSystem : AbstractExecutableSystem
    {
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
                velocityComponent.Velocity += forceComponent.Force / massComponent.Mass;
            }
        }
    }
}
