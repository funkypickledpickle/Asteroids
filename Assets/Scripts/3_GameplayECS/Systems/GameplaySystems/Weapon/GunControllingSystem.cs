using System;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;

namespace Asteroids.GameplayECS.Systems.Weapon
{
    public class GunControllingSystem : IExecutableSystem, IDisposable
    {
        private EntityGroup _armedShips;

        public GunControllingSystem(IInstanceSpawner instanceSpawner)
        {
            _armedShips = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<GunControlComponent>()
                .RequireComponent<GunComponent>()
                .Build();
        }

        public void Dispose()
        {
            _armedShips.Dispose();
            _armedShips = null;
        }

        void IExecutableSystem.Execute()
        {
            _armedShips.ForEachComponents<GunControlComponent, GunComponent>(Execute);
        }

        private static void Execute(ref Entity entity, ref GunControlComponent gunControlComponent, ref GunComponent gunComponent)
        {
            gunComponent.IsFireRequested = gunControlComponent.IsFireRequested;
        }
    }
}
