using Asteroids.Configuration.Game;
using Asteroids.Extensions;
using Asteroids.GameplayECS.Components;
using Asteroids.Services.Project;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityContainer;
using UnityEngine;
using Zenject;

namespace Asteroids.GameplayECS.Factories
{
    public class EntityFactory
    {
        [Inject] private readonly World _world;

        private readonly GameConfiguration _gameConfiguration;
        private readonly PlayerConfiguration _playerConfiguration;

        public EntityFactory(IConfigurationService configurationService)
        {
            _gameConfiguration = configurationService.Get<GameConfiguration>();
            _playerConfiguration = _gameConfiguration.PlayerConfiguration;
        }

        public void CreateGameInfo()
        {
            ref var entity = ref _world.CreateEntity();
            entity.CreateComponent<GameComponent>();
        }

        public void CreateShip(Vector3 position, float rotationDegrees)
        {
            ref var entity = ref _world.CreateEntity();
            var playerConfiguration = _playerConfiguration;
            AddFieldComponents(ref entity, position, rotationDegrees);

            entity.CreateComponent<PlayerComponent>();
            entity.CreateComponent<MainControlComponent>();

            entity.CreateComponent(new MainEngineConfigurationComponent { MaxForce = playerConfiguration.MaxAcceleration });
            entity.CreateComponent<MainEngineComponent>();
            entity.CreateComponent(new RotationEngineConfigurationComponent { MaxAngularForce = playerConfiguration.MaxAngularAcceleration });
            entity.CreateComponent<RotationEngineComponent>();

            entity.CreateComponent<UpdatableForceComponent>();
            entity.CreateComponent<UpdatableAngularForceComponent>();
            entity.CreateComponent(new MassComponent { Mass = playerConfiguration.Mass });

            entity.CreateComponent<VelocityComponent>();
            entity.CreateComponent(new VelocityLimiterComponent { MaxSpeed = playerConfiguration.MaxSpeed });
            entity.CreateComponent(new VelocityDumpComponent
            {
                StartFactor = playerConfiguration.SpeedStartDumpingFactor,
                TotalFactor = playerConfiguration.SpeedTotalDumpingFactor,
            });

            entity.CreateComponent<AngularVelocityComponent>();
            entity.CreateComponent(new AngularVelocityLimiterComponent { MaxSpeed = playerConfiguration.MaxAngularSpeed });
            entity.CreateComponent(new AngularVelocityDumpComponent
            {
                StartFactor = playerConfiguration.AngularSpeedStartDumpingFactor,
                TotalFactor = playerConfiguration.AngularSpeedTotalDumpingFactor,
            });

            entity.CreateComponent(new ViewKeyComponent { ViewKey = playerConfiguration.ViewKey });
        }

        private void AddFieldComponents(ref Entity entity, Vector2 position, float rotationDegrees)
        {
            entity.CreateComponent(new PositionComponent { Position = position });
            entity.CreateComponent(new RotationComponent { RotationDegrees = rotationDegrees });
        }
    }
}
