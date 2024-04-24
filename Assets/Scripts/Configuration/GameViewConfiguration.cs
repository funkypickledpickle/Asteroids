using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Configuration
{
    [CreateAssetMenu(menuName = "Configuration/Game View Configuration")]
    public class GameViewConfiguration : ScriptableObject
    {
        [SerializeField] private ViewKey _playerViewKey;
        [SerializeField] private ViewKey _ufoViewKey;
        [SerializeField] private ViewKey[] _asteroidsGroupsKeys;
        [SerializeField] private ViewKey _laserViewKey;
        [SerializeField] private ViewKey _bulletViewKey;

        public ViewKey PlayerViewKey => _playerViewKey;
        public ViewKey UfoViewKey => _ufoViewKey;
        public IReadOnlyList<ViewKey> AsteroidGroups => _asteroidsGroupsKeys;
        public ViewKey LaserViewKey => _laserViewKey;
        public ViewKey BulletViewKey => _bulletViewKey;
    }
}
