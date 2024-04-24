using System;
using Asteroids.GameplayECS.Components;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;

namespace Asteroids.GameplayECS.Systems.UFO
{
    public class UFODamageHandlingSystem : ISystem, IDisposable
    {
        private EntityGroup EntityGroup;

        public UFODamageHandlingSystem(IInstanceSpawner instanceSpawner)
        {
            EntityGroup = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<UFOComponent>()
                .RequireComponent<ReceivedDamageComponent>()
                .Build();

            EntityGroup.EntityAdded += HandleUFODamaged;
        }

        public void Dispose()
        {
            EntityGroup.EntityAdded -= HandleUFODamaged;
            EntityGroup.Dispose();
            EntityGroup = null;
        }

        private void HandleUFODamaged(ref Entity entity)
        {
            entity.CreateComponent<DestroyedComponent>();
        }
    }
}
