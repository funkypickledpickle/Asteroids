using System;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;

namespace Asteroids.GameplayECS.Systems.Weapon
{
    public class LaserGunControllingSystem : IExecutableSystem, IDisposable
    {
        private EntityGroup _entityGroup;

        public LaserGunControllingSystem(IInstanceSpawner instanceSpawner)
        {
            _entityGroup = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<LaserGunControlComponent>()
                .RequireComponent<LaserGunComponent>()
                .Build();
        }

        public void Dispose()
        {
            _entityGroup.Dispose();
            _entityGroup = null;
        }

        public void Execute()
        {
            _entityGroup.ForEachComponents<LaserGunControlComponent, LaserGunComponent>(Execute);
        }

        private static void Execute(ref Entity entity, ref LaserGunControlComponent controlComponent, ref LaserGunComponent gunComponent)
        {
            gunComponent.IsFireRequested = controlComponent.IsFireRequested;
        }
    }
}
