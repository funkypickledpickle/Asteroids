using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;

namespace Asteroids.GameplayECS.Systems.AngularSystems
{
    public class AngularForceResetSystem : AbstractExecutableSystem
    {
        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<UpdatableAngularForceComponent>()
               .Build();
        }

        public override void Execute()
        {
            EntityGroup.ForEachComponent<UpdatableAngularForceComponent>(Execute);
        }

        private static void Execute(ref Entity entity, ref UpdatableAngularForceComponent forceComponent)
        {
            forceComponent.AngularForce = 0;
        }
    }
}
