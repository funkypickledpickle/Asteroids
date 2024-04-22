using Asteroids.Configuration;
using Asteroids.Configuration.Project;
using Asteroids.Services.Project;
using Asteroids.Tools;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Asteroids.Services.EntityView
{
    public interface IInjectableController { }

    public class EntityViewFactory : IFactory<GameObject>
    {
        [Inject] private readonly IUnityResourcesService _resourcesService;
        [Inject] private readonly IUnitySceneService _unitySceneService;
        [Inject] private readonly IDiContainerWrapper _containerWrapper;

        private readonly string _assetPath;

        public EntityViewFactory(ViewKey viewKey, IViewConfigurationService viewConfigurationService)
        {
            _assetPath = viewConfigurationService.GetPath(viewKey);
        }

        public GameObject Create()
        {
            var instance = Object.Instantiate(_resourcesService.GetAsset<GameObject>(_assetPath), _unitySceneService.ViewContainer);
            instance.SetActive(false);

            foreach (var injectable in instance.GetComponentsInChildren<IInjectableController>(true))
            {
                _containerWrapper.Inject(injectable);
            }

            return instance;
        }
    }

    public class EntityViewMemoryPool : MemoryPool<GameObject>
    {
        protected override void OnSpawned(GameObject item)
        {
            item.SetActive(false);
        }

        protected override void OnDespawned(GameObject item)
        {
            item.SetActive(false);
        }

        protected override void OnDestroyed(GameObject item)
        {
            Object.Destroy(item);
        }
    }
}
