using System;
using Asteroids.Configuration;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Factories;
using Asteroids.GameplayECS.Systems.AI;
using Asteroids.GameplayECS.Systems.AngularSystems;
using Asteroids.GameplayECS.Systems.Asteroid;
using Asteroids.GameplayECS.Systems.Bullet;
using Asteroids.GameplayECS.Systems.Engine;
using Asteroids.GameplayECS.Systems.Laser;
using Asteroids.GameplayECS.Systems.LifeTime;
using Asteroids.GameplayECS.Systems.PositionSystems;
using Asteroids.GameplayECS.Systems.Score;
using Asteroids.GameplayECS.Systems.Ship;
using Asteroids.GameplayECS.Systems.UFO;
using Asteroids.GameplayECS.Systems.Weapon;
using Asteroids.GameplayECS.Systems.World;
using Asteroids.Services;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityContainer;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;
using UnityApplication = UnityEngine.Application;

namespace Asteroids.Gameplay.Controllers
{
    public class GameController
    {
        private readonly SystemsManager _systemsManager;
        private readonly EntityFactory _entityFactory;
        private readonly IExecutionService _executionService;
        private readonly World _world;

        public GameController(SystemsManager systemsManager, EntityFactory entityFactory, IExecutionService executionService, World world, GameConfiguration gameConfiguration, IInstanceSpawner spawner)
        {
            _systemsManager = systemsManager;
            _entityFactory = entityFactory;
            _executionService = executionService;
            _world = world;

            UnityApplication.targetFrameRate = gameConfiguration.TargetFramerate;
        }

        public void Initialize()
        {
            _systemsManager.AddSystem<ScoreCollectingSystem>();

            _systemsManager.AddSystem<ShipSpawningSystem>();
            _systemsManager.AddSystem<ShipCollisionHandlingSystem>();
            _systemsManager.AddSystem<ShipDamageHandlingSystem>();

            _systemsManager.AddSystem<UFOSpawningSystem>();
            _systemsManager.AddSystem<UFODamageHandlingSystem>();

            _systemsManager.AddSystem<AsteroidSpawningSystem>();
            _systemsManager.AddSystem<AsteroidSplittingSystem>();
            _systemsManager.AddSystem<AsteroidDamageHandlingSystem>();

            _systemsManager.AddSystem<ShipFollowingSystem>();

            _systemsManager.AddSystem<MainEngineMagicRotationControllingSystem>();
            _systemsManager.AddSystem<MainEngineControllingSystem>();
            _systemsManager.AddSystem<MainEngineRunningSystem>();
            _systemsManager.AddSystem<RotationEngineControllingSystem>();
            _systemsManager.AddSystem<RotationEngineRunningSystem>();

            _systemsManager.AddSystem<GunControllingSystem>();
            _systemsManager.AddSystem<GunShootingSystem>();

            _systemsManager.AddSystem<LaserGunControllingSystem>();
            _systemsManager.AddSystem<LaserGunShootingSystem>();
            _systemsManager.AddSystem<LaserGunAutoChargingSystem>();

            _systemsManager.AddSystem<BulletCollisionHandlingSystem>();
            _systemsManager.AddSystem<LaserCollisionHandlingSystem>();

            _systemsManager.AddSystem<WorldMirroringSystem>();

            _systemsManager.AddSystem<VelocityDumpingSystem>();
            _systemsManager.AddSystem<ForceSystem>();
            _systemsManager.AddSystem<VelocitySystem>();
            _systemsManager.AddSystem<VelocityLimiterSystem>();
            _systemsManager.AddSystem<ForceResetSystem>();

            _systemsManager.AddSystem<AngularVelocityDumpingSystem>();
            _systemsManager.AddSystem<AngularForceSystem>();
            _systemsManager.AddSystem<AngularVelocitySystem>();
            _systemsManager.AddSystem<AngularVelocityLimiterSystem>();
            _systemsManager.AddSystem<AngularForceResetSystem>();

            _systemsManager.AddSystem<LifeTimeSystem>();
            _systemsManager.AddSystem<DestroyingSystem>();
        }

        public void StartGame()
        {
            _entityFactory.CreateGameInfo();

            _executionService.FrameStarted += HandleFrameStarted;
            _executionService.ResumeExecution();
        }

        public void PauseGame()
        {
            _executionService.PauseExecution();
        }

        public void UnpauseGame()
        {
            _executionService.ResumeExecution();
        }

        public void EndGame()
        {
            _executionService.FrameStarted -= HandleFrameStarted;
            _executionService.PauseExecution();
        }

        public void Reset()
        {
            _world.Clear();
        }

        private void HandleFrameStarted()
        {
            _systemsManager.Execute();
        }
    }
}
