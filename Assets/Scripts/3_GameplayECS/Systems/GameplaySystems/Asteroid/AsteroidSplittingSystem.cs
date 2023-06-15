using Asteroids.Configuration.Game;
using Asteroids.Extensions;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Factories;
using Asteroids.Services.Project;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using UnityEngine;
using Zenject;

namespace Asteroids.GameplayECS.Systems.Asteroid
{
    public class AsteroidSplittingSystem : AbstractSystem
    {
        private const float MaxAngle = 360;
        private const float DirectionDegrees = 180;

        [Inject] private readonly EntityFactory _entityFactory;

        private readonly GameConfiguration _gameConfiguration;

        public AsteroidSplittingSystem(IConfigurationService configurationService)
        {
            _gameConfiguration = configurationService.Get<GameConfiguration>();
        }

        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<AsteroidComponent>()
               .RequireComponent<AsteroidSplitComponent>()
               .RequireComponent<PositionComponent>()
               .RequireComponent<VelocityComponent>()
               .Build();
        }

        protected override void InitializeInternal()
        {
            EntityGroup.SubscribeToEntityAddedEvent(EntityAddedHandler);
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
