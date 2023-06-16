using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;

namespace Asteroids.GameplayECS.Systems.Weapon
{
    public class LaserGunControllingSystem : AbstractExecutableSystem
    {
        private readonly ActionReference<Entity, LaserGunControlComponent, LaserGunComponent> _executeActionReference = Execute;

        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<LaserGunControlComponent>()
               .RequireComponent<LaserGunComponent>()
               .Build();
        }

        public override void Execute()
        {
            EntityGroup.ForEachComponents(_executeActionReference);
        }

        private static void Execute(ref Entity entity, ref LaserGunControlComponent controlComponent, ref LaserGunComponent gunComponent)
        {
            gunComponent.IsFireRequested = controlComponent.IsFireRequested;
        }
    }
}
