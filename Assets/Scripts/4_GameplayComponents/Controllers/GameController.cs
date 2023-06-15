using System;
using Asteroids.Configuration.Game;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Factories;
using Asteroids.GameplayECS.Systems.AngularSystems;
using Asteroids.GameplayECS.Systems.Asteroid;
using Asteroids.GameplayECS.Systems.Bullet;
using Asteroids.GameplayECS.Systems.Engine;
using Asteroids.GameplayECS.Systems.LifeTime;
using Asteroids.GameplayECS.Systems.PositionSystems;
using Asteroids.GameplayECS.Systems.Ship;
using Asteroids.GameplayECS.Systems.ViewSystems;
using Asteroids.GameplayECS.Systems.Weapon;
using Asteroids.Services.Project;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityContainer;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;
using Zenject;
using UnityApplication = UnityEngine.Application;

namespace Asteroids.GameplayComponents.Controllers
{
    public class GameController
    {
        public event EventHandler PlayerDestroyed;

        [Inject] private readonly SystemsManager _systemsManager;
        [Inject] private readonly EntityFactory _entityFactory;
        [Inject] private readonly IUnityExecutionService _executionService;
        [Inject] private readonly IUnitySceneService _sceneService;
        [Inject] private readonly World _world;

        private readonly EntityGroup _playerGroup;
        private readonly Action _executeAction;

        public GameController(IConfigurationService configurationService, IInstanceSpawner spawner)
        {
            var gameConfiguration = configurationService.Get<GameConfiguration>();
            UnityApplication.targetFrameRate = gameConfiguration.TargetFramerate;

            _playerGroup = spawner.Instantiate<EntityGroupBuilder>().RequireComponent<PlayerComponent>().Build();
            _playerGroup.SubscribeToEntityRemovedEvent(PlayerDestroyedHandler);

            _executeAction = Execute;
        }

        public void Initialize()
        {
            _systemsManager.AddSystem<ShipSpawningSystem>();
            _systemsManager.AddSystem<ShipCollisionHandlingSystem>();
            _systemsManager.AddSystem<ShipDamageHandlingSystem>();

            _systemsManager.AddSystem<AsteroidSpawningSystem>();
            _systemsManager.AddSystem<AsteroidSplittingSystem>();
            _systemsManager.AddSystem<AsteroidDamageHandlingSystem>();

            _systemsManager.AddSystem<MainEngineControllingSystem>();
            _systemsManager.AddSystem<MainEngineRunningSystem>();
            _systemsManager.AddSystem<RotationEngineControllingSystem>();
            _systemsManager.AddSystem<RotationEngineRunningSystem>();

            _systemsManager.AddSystem<GunControllingSystem>();
            _systemsManager.AddSystem<GunShootingSystem>();

            _systemsManager.AddSystem<BulletCollisionHandlingSystem>();

            _systemsManager.AddSystem<ViewManagementSystem>();
            _systemsManager.AddSystem<ViewScalingSystem>();

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

            _systemsManager.AddSystem<ViewCollisionHandlingSystem>();
            _systemsManager.AddSystem<TransformPositionUpdateSystem>();
            _systemsManager.AddSystem<TransformRotationUpdateSystem>();

            _systemsManager.AddSystem<ViewActivationSystem>();

            _systemsManager.AddSystem<LifeTimeManagementSystem>();
            _systemsManager.AddSystem<TimedDeathExecutionSystem>();
            _systemsManager.AddSystem<DestroyingSystem>();

            _systemsManager.Initialize();
        }

        public void StartGame()
        {
            _entityFactory.CreateGameInfo();

            _executionService.SubscribeToUpdate(_executeAction);
            _executionService.UnPauseUpdate();
        }

        public void PauseGame()
        {
            _executionService.PauseUpdate();
        }

        public void UnpauseGame()
        {
            _executionService.UnPauseUpdate();
        }

        public void EndGame()
        {
            _executionService.UnsubscribeFromUpdate(_executeAction);
            _executionService.PauseUpdate();
        }

        private void Execute()
        {
            _systemsManager.Execute();
        }

        private void PlayerDestroyedHandler(ref Entity referenced)
        {
            PlayerDestroyed?.Invoke(this, EventArgs.Empty);
        }
    }
}
