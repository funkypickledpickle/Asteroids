using Asteroids.Extensions;
using Asteroids.Services.Project;
using UnityEngine;
using Zenject;

namespace Asteroids.Common
{
    public class UnityObjectsInstaller : MonoBehaviour
    {
        [SerializeField] private UnitySceneService _sceneService;
        [SerializeField] private UnityExecutionService _unityExecutionService;

        public void InstallBindings(DiContainer container)
        {
            container.BindAsSingleFromInstance<IUnitySceneService>(_sceneService);
            container.BindAsSingleFromInstance(_unityExecutionService, typeof(IUnityExecutionService), typeof(IFrameInfoService));
        }
    }
}
