using Asteroids.Configuration.Project;
using UnityEngine;

namespace Asteroids.Services.Project
{
    public interface IUnitySceneService
    {
        UnityConfigurationContainer ConfigurationContainer { get; }
        Transform ViewContainer { get; }
        public Camera Camera { get; }
    }

    public class UnitySceneService : MonoBehaviour, IUnitySceneService
    {
        [SerializeField] private UnityConfigurationContainer _configurationContainer;
        [SerializeField] private Transform _viewContainer;
        [SerializeField] private Camera _camera;

        public UnityConfigurationContainer ConfigurationContainer => _configurationContainer;
        public Transform ViewContainer => _viewContainer;
        public Camera Camera => _camera;
    }
}
