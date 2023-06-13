using UnityEngine;

namespace Asteroids.Configuration.Game
{
    [CreateAssetMenu(menuName = "Configuration/Gameplay Configuration")]
    public class GameConfiguration : ScriptableObject
    {
        [SerializeField] private PlayerConfiguration _playerConfiguration;

        public PlayerConfiguration PlayerConfiguration => _playerConfiguration;
    }
}
