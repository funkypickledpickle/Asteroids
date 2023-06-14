using UnityEngine;

namespace Asteroids.Configuration.Game
{
    [CreateAssetMenu(menuName = "Configuration/Gameplay Configuration")]
    public class GameConfiguration : ScriptableObject
    {
        [SerializeField] private PlayerConfiguration _playerConfiguration;
        [SerializeField] private int _targetFramerate;

        public PlayerConfiguration PlayerConfiguration => _playerConfiguration;
        public int TargetFramerate => _targetFramerate;
    }
}
