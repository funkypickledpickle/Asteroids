using Asteroids.Gameplay.Controllers;
using Asteroids.Services;
using Asteroids.Tools;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Asteroids.Installation
{
    public class Loader : MonoBehaviour
    {
        [FormerlySerializedAs("_unityExecutionService")] [SerializeField] private ExecutionService executionService;

        private void Awake()
        {
            var container = new DiContainer();
            container.BindAsSingleFromInstance(executionService, typeof(IExecutionService), typeof(IFrameInfoService));

            container.Install<CommonInstaller>();
            container.Install<ConfigurationInstaller>();
            container.Install<ECSInstaller>();
            container.Install<ControllersInstaller>();

            container.Install<InputInstaller>();
            container.Install<ViewServicesInstaller>();

            var gameController = container.Resolve<GameController>();
            gameController.Initialize();
            gameController.StartGame();
        }
    }
}
