using Asteroids.GameplayComponents.Controllers;
using UnityEngine;
using Zenject;

namespace Asteroids.Common
{
    public class Loader : MonoBehaviour
    {
        [SerializeField] private UnityObjectsInstaller _installer;

        private void Start()
        {
            var container = new DiContainer();
            _installer.InstallBindings(container);
            container.Install<ServicesInstaller>();
            container.Install<InputInstaller>();
            container.Install<ECSInstaller>();
            container.Install<ControllersInstaller>();
            var gameController = container.Resolve<GameController>();
            gameController.Initialize();
            gameController.StartGame();
            var inputController = container.Resolve<InputController>();
            inputController.Enable();
        }
    }
}
