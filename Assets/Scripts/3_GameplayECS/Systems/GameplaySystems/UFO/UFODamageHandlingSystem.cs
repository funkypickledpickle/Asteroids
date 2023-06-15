using Asteroids.GameplayECS.Components;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;

namespace Asteroids.GameplayECS.Systems.UFO
{
    public class UFODamageHandlingSystem : AbstractSystem
    {
        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<UFOComponent>()
               .RequireComponent<ReceivedDamageComponent>()
               .Build();
        }

        protected override void InitializeInternal()
        {
            EntityGroup.SubscribeToEntityAddedEvent(AlienShipDamagedHandler);
        }

        private void AlienShipDamagedHandler(ref Entity entity)
        {
            entity.CreateComponent<DestroyedComponent>();
        }
    }
}
