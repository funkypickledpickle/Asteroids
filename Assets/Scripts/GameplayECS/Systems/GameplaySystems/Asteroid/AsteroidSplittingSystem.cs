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
            ref AsteroidComponent asteroidConfigurationComponent = ref parent.GetComponent<AsteroidComponent>();
            ref PositionComponent rootPositionComponent = ref parent.GetComponent<PositionComponent>();
            ref VelocityComponent rootVelocityComponent = ref parent.GetComponent<VelocityComponent>();

            int groupConfigurationIndex = asteroidConfigurationComponent.GroupConfigurationIndex;
            int stateIndex = asteroidConfigurationComponent.StateIndex + 1;

            AsteroidGroupConfiguration groupConfiguration = _gameConfiguration.AsteroidGroupConfigurations[groupConfigurationIndex];
            if (stateIndex >= groupConfiguration.AsteroidStates.Count)
            {
                return;
            }

            AsteroidGroupConfiguration.StateInfo asteroidStateInfo = groupConfiguration.AsteroidStates[stateIndex];
            AsteroidConfiguration asteroidConfiguration = asteroidStateInfo.AsteroidConfiguration;
            int quantity = asteroidStateInfo.Quantity;

            Quaternion velocityQuaternion = Quaternion.FromToRotation(Vector3.up, rootVelocityComponent.Velocity.normalized);
            float velocityAngleDegrees = velocityQuaternion.eulerAngles.z;
            float minAngle = velocityAngleDegrees - DirectionDegrees / 2;
            float maxAngle = velocityAngleDegrees + DirectionDegrees / 2;
            float difference = DirectionDegrees / quantity;

            for (float i = minAngle + difference / 2; i < maxAngle; i += difference)
            {
                Quaternion rotation = Quaternion.Euler(0, 0, i);
                Vector3 direction = (rotation * Vector3.up).normalized;

                float targetRotationDegrees = Random.Range(0, MaxAngle);
                float targetAngularSpeed = asteroidConfiguration.MinMaxAngularSpeedDegrees.RandomRange();
                Vector3 targetVelocity = rootVelocityComponent.Velocity.magnitude * direction * asteroidStateInfo.SpeedMultiplier;
                _entityFactory.CreateMeteorite(groupConfigurationIndex, stateIndex, rootPositionComponent.Position, targetRotationDegrees, targetVelocity, targetAngularSpeed, asteroidStateInfo);
            }
        }
    }
}
