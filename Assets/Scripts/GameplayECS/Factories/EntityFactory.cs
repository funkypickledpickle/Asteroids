using Asteroids.Configuration;
using Asteroids.GameplayECS.Components;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityContainer;
using UnityEngine;

namespace Asteroids.GameplayECS.Factories
{
    public class EntityFactory
    {
        private readonly World _world;
        private readonly GameConfiguration _gameConfiguration;
        private readonly BulletConfiguration _bulletConfiguration;
        private readonly PlayerConfiguration _playerConfiguration;
        private readonly UFOConfiguration _ufoConfiguration;
        private readonly LaserConfiguration _laserConfiguration;
        private readonly FieldConfiguration _fieldConfiguration;

        public EntityFactory(World world, GameConfiguration gameConfiguration, FieldConfiguration fieldConfiguration)
        {
            _world = world;
            _gameConfiguration = gameConfiguration;
            _fieldConfiguration = fieldConfiguration;
            _bulletConfiguration = _gameConfiguration.BulletConfiguration;
            _playerConfiguration = _gameConfiguration.PlayerConfiguration;
            _ufoConfiguration = _gameConfiguration.UfoConfiguration;
            _laserConfiguration = _gameConfiguration.LaserConfiguration;
        }

        public void CreateGameInfo()
        {
            var gameConfiguration = _gameConfiguration;
            ref var entity = ref _world.CreateEntity();
            entity.CreateComponent<GameComponent>();
            entity.CreateComponent<ScoreComponent>();

            var worldRect = _fieldConfiguration.Rect;
            var min = worldRect.min - Vector2.one * gameConfiguration.WorldBoundsPadding;
            var size = worldRect.size + Vector2.one * 2 * gameConfiguration.WorldBoundsPadding;
            var bounds = new Rect(min, size);
            entity.CreateComponent(new WorldBoundsComponent { Bounds = bounds });
        }

        public void CreateAsteroidSpawningTimer(float duration)
        {
            ref var entity = ref _world.CreateEntity();
            entity.CreateComponent<AsteroidSpawningTimerComponent>();
            entity.CreateComponent(new LifeTimeComponent()
            {
                Duration = duration,
            });
        }

        public void CreateMeteorite(int groupConfigurationIndex, int stateIndex, Vector2 position, float rotationDegrees, Vector3 velocity, float angularSpeed, AsteroidGroupConfiguration.StateInfo stateInfo)
        {
            ref var entity = ref CreateAsteroidInternal(groupConfigurationIndex, stateIndex, position, rotationDegrees, velocity, angularSpeed, stateInfo);
            entity.CreateComponent<MeteoriteComponent>();
        }

        public void CreateAsteroid(int groupConfigurationIndex, int stateIndex, Vector2 position, float rotationDegrees, Vector2 velocity, float angularSpeed, AsteroidGroupConfiguration.StateInfo stateInfo)
        {
            CreateAsteroidInternal(groupConfigurationIndex, stateIndex, position, rotationDegrees, velocity, angularSpeed, stateInfo);
        }

        private ref Entity CreateAsteroidInternal(int groupConfigurationIndex, int stateIndex, Vector2 position, float rotationDegrees, Vector2 velocity, float angularSpeed, AsteroidGroupConfiguration.StateInfo stateInfo)
        {
            ref var entity = ref _world.CreateEntity();
            entity.CreateComponent(new AsteroidComponent()
            {
                GroupConfigurationIndex = groupConfigurationIndex,
                StateIndex = stateIndex
            });

            AddFieldComponents(ref entity, position, rotationDegrees);

            entity.CreateComponent(new VelocityComponent { Velocity = velocity });
            entity.CreateComponent(new AngularVelocityComponent { AngularSpeed = angularSpeed });
            entity.CreateComponent(new ScaleComponent { Scale = Vector3.one * stateInfo.Size });
            entity.CreateComponent<WorldMirroringAfterWorldBoundsEntryComponent>();
            entity.CreateComponent(new RewardableScoreComponent { Score = stateInfo.RewardedScore });
            return ref entity;
        }

        public void CreateBullet(Vector2 position, float rotationDegrees, Vector2 speed)
        {
            var bulletConfiguration = _bulletConfiguration;
            ref var entity = ref _world.CreateEntity();
            AddFieldComponents(ref entity, position, rotationDegrees);

            entity.CreateComponent<BulletComponent>();
            entity.CreateComponent(new VelocityComponent { Velocity = speed });
            entity.CreateComponent(new LifeTimeComponent { Duration = bulletConfiguration.LifeTime });
            entity.CreateComponent<WorldMirroringComponent>();
        }

        public void CreateLaser(int ownerId, Vector2 positionOffset, float distance)
        {
            var laserConfiguration = _laserConfiguration;
            ref var entity = ref _world.CreateEntity();

            entity.CreateComponent<LaserComponent>();
            entity.CreateComponent(new AttachedToEntityComponent() { EntityId = ownerId, PositionOffset = positionOffset });
            entity.CreateComponent(new LifeTimeComponent { Duration = laserConfiguration.Lifetime });
            entity.CreateComponent(new ScaleComponent { Scale = new Vector3(1, distance, 1) });
        }

        public void CreateShip(Vector3 position, float rotationDegrees)
        {
            ref var entity = ref _world.CreateEntity();
            var playerConfiguration = _playerConfiguration;
            AddFieldComponents(ref entity, position, rotationDegrees);

            entity.CreateComponent<PlayerComponent>();
            entity.CreateComponent<ShipComponent>();
            entity.CreateComponent<MainControlComponent>();

            entity.CreateComponent(new MainEngineConfigurationComponent { MaxForce = playerConfiguration.MaxAcceleration });
            entity.CreateComponent<MainEngineComponent>();
            entity.CreateComponent(new RotationEngineConfigurationComponent { MaxAngularForce = playerConfiguration.MaxAngularAcceleration });
            entity.CreateComponent<RotationEngineComponent>();

            entity.CreateComponent(new GunComponent()
            {
                Configuration = new GunConfigurationComponent
                {
                    BulletSpeed = _bulletConfiguration.BulletSpeed,
                    FiringInterval = playerConfiguration.GunFiringInterval,
                    BulletSpawnPositionOffset = playerConfiguration.BulletSpawnPositionOffset
                }
            });
            entity.CreateComponent<GunControlComponent>();
            entity.CreateComponent(new LaserGunComponent()
            {
                Configuration = new LaserGunConfigurationComponent
                {
                    FiringInterval = playerConfiguration.LaserGunFiringInterval,
                    LaserSpawnPositionOffset = playerConfiguration.LaserSpawnPositionOffset,
                    Distance = playerConfiguration.LaserDistance,
                },
                ChargesCount = playerConfiguration.InitialChargesQuantity,
            });
            entity.CreateComponent<LaserGunControlComponent>();
            entity.CreateComponent(new LaserAutoChargingComponent
            {
                Configuration = new LaserAutoChargingConfigurationComponent
                {
                    Duration = playerConfiguration.LaserChargeLoadingDuration,
                    MaxChargesQuantity = playerConfiguration.MaxChargesQuantity,
                }
            });

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

            entity.CreateComponent<WorldMirroringComponent>();
        }

        public void CreateUFO(Vector3 position)
        {
            var ufoConfiguration = _ufoConfiguration;
            ref var entity = ref _world.CreateEntity();
            entity.CreateComponent<UFOComponent>();
            entity.CreateComponent<ShipFollowerComponent>();
            entity.CreateComponent<MainControlComponent>();

            entity.CreateComponent(new MassComponent { Mass = ufoConfiguration.Mass });

            entity.CreateComponent(new MainEngineConfigurationComponent { MaxForce = ufoConfiguration.MaxAcceleration });
            entity.CreateComponent<MainEngineComponent>();
            entity.CreateComponent<MainEngineMagicRotationComponent>();

            entity.CreateComponent<UpdatableForceComponent>();
            AddFieldComponents(ref entity, position, 0);
            entity.CreateComponent<VelocityComponent>();
            entity.CreateComponent(new VelocityLimiterComponent { MaxSpeed = ufoConfiguration.SpeedRange.RandomRange() });
            entity.CreateComponent(new RewardableScoreComponent { Score = ufoConfiguration.RewardedScore });
        }

        public void CreateUFOSpawningTimer(float duration)
        {
            ref var entity = ref _world.CreateEntity();
            entity.CreateComponent<UFOSpawningTimerComponent>();
            entity.CreateComponent(new LifeTimeComponent()
            {
                Duration = duration,
            });
        }

        public void CreateRewardedScoreEntity(int score)
        {
            ref var entity = ref _world.CreateEntity();
            entity.CreateComponent(new ReceivedScoreComponent { Score = score });
        }

        private void AddFieldComponents(ref Entity entity, Vector2 position, float rotationDegrees)
        {
            entity.CreateComponent(new PositionComponent { Position = position });
            entity.CreateComponent(new RotationComponent { RotationDegrees = rotationDegrees });
        }
    }
}
