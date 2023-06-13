using Asteroids.Configuration.Project;
using UnityEngine;

namespace Asteroids.Services.Project
{
    public interface IUnitySceneService
    {
        UnityConfigurationContainer ConfigurationContainer { get; }
        Transform ViewContainer { get; }
    }

    public class UnitySceneService : MonoBehaviour, IUnitySceneService
    {
        [SerializeField] private UnityConfigurationContainer _configurationContainer;
        [SerializeField] private Transform _viewContainer;

        public UnityConfigurationContainer ConfigurationContainer => _configurationContainer;
        public Transform ViewContainer => _viewContainer;
    }
}
