using System;
using Asteroids.Configuration;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Factories;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids.GameplayECS.Systems.Asteroid
{
    public class AsteroidSplittingSystem : ISystem, IDisposable
    {
        private const float MaxAngle = 360;
        private const float DirectionDegrees = 180;

        private readonly EntityFactory _entityFactory;
        private readonly GameConfiguration _gameConfiguration;

        private readonly EntityGroup _entityGroup;

        public AsteroidSplittingSystem(IInstanceSpawner instanceSpawner, GameConfiguration gameConfiguration, EntityFactory entityFactory)
        {
            _gameConfiguration = gameConfiguration;
            _entityFactory = entityFactory;

            _entityGroup = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<AsteroidComponent>()
                .RequireComponent<AsteroidSplitComponent>()
                .RequireComponent<PositionComponent>()
                .RequireComponent<VelocityComponent>()
                .Build();
            _entityGroup.EntityAdded += EntityAddedHandler;
        }

        public void Dispose()
        {
            _entityGroup.EntityAdded -= EntityAddedHandler;
            _entityGroup.Dispose();
        }

        private void EntityAddedHandler(ref Entity entity)
        {
            SpawnFragments(ref entity);
        }

        private void SpawnFragments(ref Entity parent)
        {
            ref var asteroidConfigurationComponent = ref parent.GetComponent<AsteroidComponent>();
            ref var rootPositionComponent = ref parent.GetComponent<PositionComponent>();
            ref var rootVelocityComponent = ref parent.GetComponent<VelocityComponent>();

            var groupConfigurationIndex = asteroidConfigurationComponent.GroupConfigurationIndex;
            var stateIndex = asteroidConfigurationComponent.StateIndex + 1;

            var groupConfiguration = _gameConfiguration.AsteroidGroupConfigurations[groupConfigurationIndex];
            if (stateIndex >= groupConfiguration.AsteroidStates.Count)
            {
                return;
            }

            var asteroidStateInfo = groupConfiguration.AsteroidStates[stateIndex];
            var asteroidConfiguration = asteroidStateInfo.AsteroidConfiguration;
            var quantity = asteroidStateInfo.Quantity;

            var velocityQuaternion = Quaternion.FromToRotation(Vector3.up, rootVelocityComponent.Velocity.normalized);
            var velocityAngleDegrees = velocityQuaternion.eulerAngles.z;
            var minAngle = velocityAngleDegrees - DirectionDegrees / 2;
            var maxAngle = velocityAngleDegrees + DirectionDegrees / 2;
            var difference = DirectionDegrees / quantity;

            for (var i = minAngle + difference / 2; i < maxAngle; i += difference)
            {
                var rotation = Quaternion.Euler(0, 0, i);
                var direction = (rotation * Vector3.up).normalized;

                var targetRotationDegrees = Random.Range(0, MaxAngle);
                var targetAngularSpeed = asteroidConfiguration.MinMaxAngularSpeedDegrees.RandomRange();
                var targetVelocity = rootVelocityComponent.Velocity.magnitude * direction * asteroidStateInfo.SpeedMultiplier;
                _entityFactory.CreateMeteorite(groupConfigurationIndex, stateIndex, rootPositionComponent.Position, targetRotationDegrees, targetVelocity, targetAngularSpeed, asteroidStateInfo);
            }
        }
    }
}
