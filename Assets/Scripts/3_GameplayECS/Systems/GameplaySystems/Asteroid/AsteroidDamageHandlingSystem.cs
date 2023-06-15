using Asteroids.GameplayECS.Components;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;

namespace Asteroids.GameplayECS.Systems.Asteroid
{
    public class AsteroidDamageHandlingSystem : AbstractSystem
    {
        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<AsteroidComponent>()
               .RequireComponent<ReceivedDamageComponent>()
               .Build();
        }

        protected override void InitializeInternal()
        {
            EntityGroup.SubscribeToEntityAddedEvent(AsteroidDamagedHandler);
        }

        private void AsteroidDamagedHandler(ref Entity entity)
        {
            entity.CreateComponent<DestroyedComponent>();
            entity.CreateComponent<AsteroidSplitComponent>();
        }
    }
}
