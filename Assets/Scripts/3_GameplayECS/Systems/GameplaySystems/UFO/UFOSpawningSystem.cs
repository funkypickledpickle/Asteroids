using System;
using Asteroids.Configuration.Game;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.GameplayECS.Factories;
using Asteroids.Services.Project;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Asteroids.GameplayECS.Systems.UFO
{
    public class UFOSpawningSystem : AbstractSystem
    {
        private const float MaxAngle = 360;

        [Inject] private readonly EntityFactory _entityFactory;
        [Inject] private readonly IActionSchedulingService _actionSchedulingService;
        [Inject] private readonly IFrameInfoService _frameInfoService;

        private readonly GameConfiguration _gameConfiguration;
        private readonly UFOConfiguration _ufoConfiguration;
        private EntityGroup _ufoGroup;

        public UFOSpawningSystem(IConfigurationService configurationService)
        {
            _gameConfiguration = configurationService.Get<GameConfiguration>();
            _ufoConfiguration = _gameConfiguration.UfoConfiguration;
        }

        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<ShipComponent>()
               .Build();
        }

        protected override void InitializeInternal()
        {
            _ufoGroup = InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<UFOComponent>()
               .Build();
            EntityGroup.SubscribeToEntityAddedEvent(EntityAdded);
        }

        private void EntityAdded(ref Entity entity)
        {
            Execute();
        }

        private void Execute()
        {
            TryCreateAlien();
            ScheduleCreation();
        }

        private void TryCreateAlien()
        {
            var quantity = _gameConfiguration.MaxUfoQuantity - _ufoGroup.Count;
            if (quantity > 0)
            {
                CreateAlienShips(quantity);
            }
        }

        private void CreateAlienShips(int quantity)
        {
            if (EntityGroup.Count != 0)
            {
                ref var entity = ref EntityGroup.GetFirst();
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

        private void ScheduleCreation()
        {
            var targetTime = _frameInfoService.StartTime + _gameConfiguration.UfoSpawnInterval;
            _actionSchedulingService.Schedule(targetTime, Execute);
        }
    }
}
