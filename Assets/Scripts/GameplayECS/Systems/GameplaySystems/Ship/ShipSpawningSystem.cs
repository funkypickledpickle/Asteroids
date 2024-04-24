using System;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Factories;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;
using UnityEngine;

namespace Asteroids.GameplayECS.Systems.Ship
{
    public class ShipSpawningSystem : ISystem, IDisposable
    {
        private readonly EntityFactory _entityFactory;

        private EntityGroup _gameEntitiesGroup;

        public ShipSpawningSystem(EntityFactory entityFactory, IInstanceSpawner instanceSpawner)
        {
            _entityFactory = entityFactory;

            _gameEntitiesGroup = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<GameComponent>()
                .Build();
            _gameEntitiesGroup.EntityAdded += HandleGameEntityAdded;
        }

        public void Dispose()
        {
            _gameEntitiesGroup.EntityAdded -= HandleGameEntityAdded;
            _gameEntitiesGroup.Dispose();
            _gameEntitiesGroup = null;
        }

        private void HandleGameEntityAdded(ref Entity entity)
        {
            _entityFactory.CreateShip(Vector3.zero, 0);
        }
    }
}
