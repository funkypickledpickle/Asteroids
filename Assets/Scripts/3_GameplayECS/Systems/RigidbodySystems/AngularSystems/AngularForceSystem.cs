using Asteroids.GameplayECS.Components;
using Asteroids.ValueTypeECS.EntityGroup;

namespace Asteroids.GameplayECS.Systems.AngularSystems
{
    public class AngularForceSystem : AbstractExecutableSystem
    {
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
                velocityComponent.AngularSpeed += forceComponent.AngularForce / massComponent.Mass;
            }
        }
    }
}
