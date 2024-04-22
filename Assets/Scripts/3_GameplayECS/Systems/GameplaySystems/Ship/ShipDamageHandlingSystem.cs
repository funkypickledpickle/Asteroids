using System;
using Asteroids.GameplayECS.Components;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;

namespace Asteroids.GameplayECS.Systems.Ship
{
    public class ShipDamageHandlingSystem : ISystem, IDisposable
    {
        private EntityGroup _damagedShipsGroup;

        public ShipDamageHandlingSystem(IInstanceSpawner instanceSpawner)
        {
            _damagedShipsGroup = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<ShipComponent>()
                .RequireComponent<ReceivedDamageComponent>().Build();

            _damagedShipsGroup.EntityAdded += HandleShipDamaged;
        }

        public void Dispose()
        {
            _damagedShipsGroup.EntityAdded -= HandleShipDamaged;
            _damagedShipsGroup.Dispose();
            _damagedShipsGroup = null;
        }

        private void HandleShipDamaged(ref Entity entity)
        {
            entity.CreateComponent<DestroyedComponent>();
        }
    }
}
