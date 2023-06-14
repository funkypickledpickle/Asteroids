using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Factories;
using Asteroids.Services.Project;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using UnityEngine;
using Zenject;

namespace Asteroids.GameplayECS.Systems.Ship
{
    public class ShipSpawningSystem : AbstractSystem
    {
        [Inject] private readonly EntityFactory _entityFactory;

        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<GameComponent>()
               .Build();
        }

        protected override void InitializeInternal()
        {
            EntityGroup.SubscribeToEntityAddedEvent(EntityAdded);
        }

        private void EntityAdded(ref Entity entity)
        {
            _entityFactory.CreateShip(Vector3.zero, 0);
        }
    }
}
