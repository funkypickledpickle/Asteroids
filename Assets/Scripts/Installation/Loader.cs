using System;
using Asteroids.Gameplay.Controllers;
using Asteroids.Services;
using Asteroids.Services.EntityView;
using Asteroids.Tools;
using Asteroids.UI.Behaviours;
using UnityEngine;
using Zenject;

namespace Asteroids.Installation
{
    public class Loader : MonoBehaviour
    {
        [SerializeField] private ExecutionService _executionService;
        [SerializeField] private GameCamera _gameCamera;
        [SerializeField] private UnitFactory unitFactory;
        [SerializeField] private UnitSpawner unitSpawner;

        private void Awake()
        {
            var container = new DiContainer();
            container.BindAsSingleFromInstance<IGameWorld>(_gameCamera);
            container.BindAsSingleFromInstance(_executionService, typeof(IExecutionService), typeof(IFrameInfoService));

            container.Install<CommonInstaller>();
            container.Install<ConfigurationInstaller>();
            container.Install<ECSInstaller>();
            container.Install<ControllersInstaller>();

            container.BindAsSingleFromInstance<UnitFactory>(unitFactory);
            container.BindAsSingleFromInstance<UnitSpawner>(unitSpawner);
            container.Install<InputInstaller>();
            container.Install<ViewServicesInstaller>();

            ContainersRegistry.AddContainer(Containers.SceneContainerId, container);
        }

        private void Start()
        {
            var container = ContainersRegistry.GetContainer(Containers.SceneContainerId);
            var gameController = container.Resolve<GameController>();
            gameController.Initialize();
            gameController.StartGame();
        }

        private void OnDestroy()
        {
            ContainersRegistry.RemoveContainer(Containers.SceneContainerId);
        }
    }
}
