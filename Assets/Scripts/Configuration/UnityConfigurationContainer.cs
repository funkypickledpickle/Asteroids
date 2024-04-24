using UnityEngine;
using UnityEngine.Serialization;

namespace Asteroids.Configuration
{
    [CreateAssetMenu(menuName = "Configuration/Configuration Container")]
    public class UnityConfigurationContainer : ScriptableObject
    {
        [SerializeField] private ViewPathsContainer _viewPathsContainer;
        [SerializeField] private GameConfiguration _gameConfiguration;
        [SerializeField] private ContainersConfiguration _containersConfiguration;
        [FormerlySerializedAs("preloadConfiguration")] [FormerlySerializedAs("_preloadingConfiguration")] [FormerlySerializedAs("preloadingConfiguration")] [FormerlySerializedAs("_warmupConfiguration")] [SerializeField] private GamePreloadConfiguration _preloadConfiguration;

        public ViewPathsContainer ViewPathsContainer => _viewPathsContainer;
        public GameConfiguration GameConfiguration => _gameConfiguration;
        public ContainersConfiguration ContainersConfiguration => _containersConfiguration;
        public GamePreloadConfiguration PreloadConfiguration => _preloadConfiguration;
    }
}
