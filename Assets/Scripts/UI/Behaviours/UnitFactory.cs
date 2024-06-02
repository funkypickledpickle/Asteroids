using Asteroids.Configuration;
using Asteroids.Installation;
using Asteroids.Configuration.Project;
using Asteroids.Services.Project;
using Asteroids.UI;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Asteroids.Services.EntityView
{
    [RequireComponent(typeof(Injector))]
    public class UnitFactory : MonoBehaviour, IInjectable
    {
        [SerializeField] private Transform _entityViewContainer;

        [Inject] private IViewConfigurationService _viewConfigurationService;
        [Inject] private IResourcesService _resourcesService;

        public GameObject Create(ViewKey viewKey)
        {
            string assetPath = _viewConfigurationService.GetPath(viewKey);
            GameObject instance = Object.Instantiate(_resourcesService.GetAsset<GameObject>(assetPath), _entityViewContainer);
            instance.SetActive(false);
            return instance;
        }
    }
}
