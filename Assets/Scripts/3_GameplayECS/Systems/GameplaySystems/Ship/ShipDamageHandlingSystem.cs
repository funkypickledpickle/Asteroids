using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;

namespace Asteroids.GameplayECS.Systems.Ship
{
    public class ShipDamageHandlingSystem : AbstractSystem
    {
        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<ShipComponent>()
               .RequireComponent<ReceivedDamageComponent>().Build();
        }

        protected override void InitializeInternal()
        {
            EntityGroup.SubscribeToEntityAddedEvent(EntityDamagedHandler);
        }

        private void EntityDamagedHandler(ref Entity entity)
        {
            entity.CreateComponent<DestroyedComponent>();
        }
    }
}
