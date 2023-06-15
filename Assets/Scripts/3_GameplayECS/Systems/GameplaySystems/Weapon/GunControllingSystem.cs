using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;

namespace Asteroids.GameplayECS.Systems.Weapon
{
    public class GunControllingSystem : AbstractExecutableSystem
    {
        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<GunControlComponent>()
               .RequireComponent<GunComponent>()
               .Build();
        }

        public override void Execute()
        {
            EntityGroup.ForEachComponents<GunControlComponent, GunComponent>(Execute);
        }

        private static void Execute(ref Entity entity, ref GunControlComponent gunControlComponent, ref GunComponent gunComponent)
        {
            gunComponent.IsFireRequested = gunControlComponent.IsFireRequested;
        }
    }
}
