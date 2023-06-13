using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using UnityEngine;

namespace Asteroids.GameplayECS.Systems.PositionSystems
{
    public class ForceResetSystem : AbstractExecutableSystem
    {
        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<UpdatableForceComponent>()
               .Build();
        }

        public override void Execute()
        {
            EntityGroup.ForEachComponent<UpdatableForceComponent>(Execute);
        }

        private static void Execute(ref Entity entity, ref UpdatableForceComponent forceComponent)
        {
            forceComponent.Force = Vector2.zero;
        }
    }
}
