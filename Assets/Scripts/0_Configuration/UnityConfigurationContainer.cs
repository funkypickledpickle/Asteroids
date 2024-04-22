using UnityEngine;

namespace Asteroids.Configuration
{
    [CreateAssetMenu(menuName = "Configuration/Configuration Container")]
    public class UnityConfigurationContainer : ScriptableObject
    {
        [SerializeField] private ViewPathsContainer _viewPathsContainer;
        [SerializeField] private GameConfiguration _gameConfiguration;
        [SerializeField] private ContainersConfiguration _containersConfiguration;

        public ViewPathsContainer ViewPathsContainer => _viewPathsContainer;
        public GameConfiguration GameConfiguration => _gameConfiguration;
        public ContainersConfiguration ContainersConfiguration => _containersConfiguration;
    }
}
