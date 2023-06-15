using System;
using System.Linq;
using Asteroids.Configuration.Game;
using Asteroids.Extensions;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.GameplayECS.Factories;
using Asteroids.Services.Project;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Asteroids.GameplayECS.Systems.Asteroid
{
    public class AsteroidSpawningSystem : AbstractSystem
    {
        private const float MaxAngle = 360;

        [Inject] private readonly IConfigurationService _configurationService;
        [Inject] private readonly EntityFactory _entityFactory;
        [Inject] private readonly IFrameInfoService _frameInfoService;
        [Inject] private readonly IActionSchedulingService _actionSchedulingService;

        private GameConfiguration _gameConfiguration;
        private float[] _asteroidSpawnProbabilitiesCached;
        private float _asteroidSpawnProbabilitiesSum;
        private int _intervalEntityId;

        private EntityGroup _asteroidsGroup;

        public AsteroidSpawningSystem(IConfigurationService configurationService)
        {
            _gameConfiguration = configurationService.Get<GameConfiguration>();
            _asteroidSpawnProbabilitiesCached = _gameConfiguration.AsteroidGroupConfigurations.Select(e => e.SpawnProbability).ToArray();
            _asteroidSpawnProbabilitiesSum = _asteroidSpawnProbabilitiesCached.Sum();
        }

        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<ShipComponent>()
               .Build();
        }

        protected override void InitializeInternal()
        {
            _asteroidsGroup = InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<AsteroidComponent>()
               .RequireComponentAbsence<MeteoriteComponent>()
               .Build();
            EntityGroup.SubscribeToEntityAddedEvent(EntityAdded);
        }

        private int GetRandomAsteroidIndex()
        {
            var totalProbability = 0f;
            var targetProbability = Random.Range(0, _asteroidSpawnProbabilitiesSum);
            for (var i = 0; i < _asteroidSpawnProbabilitiesCached.Length; i++)
            {
                totalProbability += _asteroidSpawnProbabilitiesCached[i];
                if (targetProbability < totalProbability)
                {
                    return i;
                }
            }

            return _asteroidSpawnProbabilitiesCached.Length - 1;
        }

        private void EntityAdded(ref Entity entity)
        {
            Execute();
        }

        private void Execute()
        {
            TryCreateAsteroids();
            ScheduleCreation();
        }

        private void TryCreateAsteroids()
        {
            var quantity = _gameConfiguration.MaxAsteroidQuantity - _asteroidsGroup.Count;
            if (quantity > 0)
            {
                CreateAsteroids(quantity);
            }
        }

        private void ScheduleCreation()
        {
            var targetTime = _frameInfoService.StartTime + _gameConfiguration.AsteroidSpawnInterval;
            _actionSchedulingService.Schedule(targetTime, Execute);
        }

        private void CreateAsteroids(int quantity)
        {
            if (EntityGroup.Count == 0)
            {
                return;
            }

            ref var shipEntity = ref EntityGroup.GetFirst();
            for (var i = 0; i < quantity; i++)
            {
                ref var positionComponent = ref shipEntity.GetComponent<PositionComponent>();

                while (_asteroidsGroup.Count < _gameConfiguration.MaxAsteroidQuantity)
                {
                    var groupConfigurationIndex = GetRandomAsteroidIndex();
                    var groupConfiguration = _gameConfiguration.AsteroidGroupConfigurations[groupConfigurationIndex];
                    var stateIndex = 0;
                    var asteroidStateInfo = groupConfiguration.AsteroidStates[stateIndex];
                    var asteroidConfiguration = asteroidStateInfo.AsteroidConfiguration;

                    var rotation = Quaternion.Euler(0, 0, Random.Range(0, MaxAngle));
                    var direction = (rotation * Vector3.up).normalized;
                    Vector2 offset = direction * asteroidConfiguration.MinMaxDistanceFromTarget.RandomRange();

                    var targetPosition = positionComponent.Position + offset;
                    Vector3 directionToTarget = (positionComponent.Position - targetPosition).normalized;
                    var targetOffsetAngle = asteroidConfiguration.MinMaxDiversionFromTargetDegrees.RandomRange() * RandomExtensions.RandomSign();
                    var directionOffset = Quaternion.Euler(0, 0, targetOffsetAngle);
                    var targetVelocityDirection = directionOffset * directionToTarget;
                    var targetVelocity = targetVelocityDirection * asteroidConfiguration.MinMaxVelocity.RandomRange();

                    var targetRotationDegrees = Random.Range(0, MaxAngle);
                    var targetAngularSpeed = asteroidConfiguration.MinMaxAngularSpeedDegrees.RandomRange();
                    _entityFactory.CreateAsteroid(groupConfigurationIndex, stateIndex, targetPosition, targetRotationDegrees, targetVelocity, targetAngularSpeed, asteroidStateInfo);
                }
            }
        }
    }
}
