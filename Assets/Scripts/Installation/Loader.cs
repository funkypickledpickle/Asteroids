using Asteroids.Gameplay.States;
using Asteroids.Services;
using Asteroids.Services.EntityView;
using Asteroids.Services.Project;
using Asteroids.Tools;
using Asteroids.UI.Behaviours;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Asteroids.Installation
{
    public class Loader : MonoBehaviour
    {
        [FormerlySerializedAs("_unityExecutionService")] [SerializeField] private ExecutionService executionService;
        [SerializeField] private GameCamera _gameCamera;
        [SerializeField] private UnitFactory unitFactory;
        [SerializeField] private UnitSpawner unitSpawner;

        private void Awake()
        {
            DiContainer container = new DiContainer();
            container.BindAsSingleFromInstance<IGameWorld>(_gameCamera);
            container.BindAsSingleFromInstance(executionService, typeof(IExecutionService), typeof(IFrameInfoService));

            container.Install<CommonInstaller>();
            container.Install<ConfigurationInstaller>();
            container.Install<ECSInstaller>();
            container.Install<ControllersInstaller>();
            container.Install<GameStatesInstaller>();

            container.BindAsSingleFromInstance<UnitFactory>(unitFactory);
            container.BindAsSingleFromInstance<UnitSpawner>(unitSpawner);
            container.Install<InputInstaller>();
            container.Install<ViewServicesInstaller>();

            ContainersRegistry.AddContainer(Containers.SceneContainerId, container);

            IStateContext gameStateContext = container.Resolve<IStateContext>();
            gameStateContext.SwitchState<InitializationState>();
        }

        private void OnDestroy()
        {
            ContainersRegistry.RemoveContainer(Containers.SceneContainerId);
        }
    }
}
