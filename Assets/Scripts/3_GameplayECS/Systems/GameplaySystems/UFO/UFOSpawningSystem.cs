using System;
using System.Threading;
using Asteroids.Configuration;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.GameplayECS.Factories;
using Asteroids.Services;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids.GameplayECS.Systems.UFO
{
    public class UFOSpawningSystem : ISystem, IDisposable
    {
        private const float MaxAngle = 360;

        private readonly EntityFactory _entityFactory;
        private readonly IFrameInfoService _frameInfoService;


        private readonly GameConfiguration _gameConfiguration;
        private readonly UFOConfiguration _ufoConfiguration;

        private EntityGroup _ships;
        private EntityGroup _ufo;
        private EntityGroup _timers;

        public UFOSpawningSystem(EntityFactory entityFactory, IFrameInfoService frameInfoService, GameConfiguration gameConfiguration, IInstanceSpawner instanceSpawner)
        {
            _entityFactory = entityFactory;
            _frameInfoService = frameInfoService;

            _gameConfiguration = gameConfiguration;
            _ufoConfiguration = _gameConfiguration.UfoConfiguration;

            _ships = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<ShipComponent>()
                .Build();

            _ufo = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<UFOComponent>()
                .Build();

            _timers = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<UFOSpawningTimerComponent>()
                .Build();

            _ships.EntityAdded += HandleShipAdded;
            _timers.EntityRemoved += HandleTimerEnded;
        }

        public void Dispose()
        {
            _ships.EntityAdded -= HandleShipAdded;
            _ships.Dispose();
            _ships = null;

            _ufo.Dispose();
            _ufo = null;

            _timers.EntityRemoved -= HandleTimerEnded;
        }

        private void HandleShipAdded(ref Entity entity)
        {
            TryCreateAndScheduleUFOCreation();
        }

        private void HandleTimerEnded(ref Entity referenced)
        {
            TryCreateAndScheduleUFOCreation();
        }

        private void TryCreateAndScheduleUFOCreation()
        {
            TryCreateUFO();
            ScheduleCreation();
        }

        private void TryCreateUFO()
        {
            var quantity = _gameConfiguration.MaxUfoQuantity - _ufo.Count;
            if (quantity > 0)
            {
                CreateAlienShips(quantity);
            }
        }

        private void ScheduleCreation()
        {
            _entityFactory.CreateUFOSpawningTimer(_gameConfiguration.UfoSpawnInterval);
        }

        private void CreateAlienShips(int quantity)
        {
            if (_ships.Count != 0)
            {
                ref var entity = ref _ships.GetFirst();
                for (var i = 0; i < quantity; i++)
                {
                    var eulerAngles = Vector3.forward * Random.Range(0, MaxAngle);
                    var rotation = Quaternion.Euler(eulerAngles);
                    var direction = rotation * Vector3.up;
                    var offset = direction * _ufoConfiguration.MaxDistanceFromTarget;
                    Vector3 targetPosition = entity.GetComponent<PositionComponent>().Position + (Vector2)offset;
                    _entityFactory.CreateUFO(targetPosition);
                }
            }
        }
    }
}
