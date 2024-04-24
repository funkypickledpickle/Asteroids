using UnityEngine;

namespace Asteroids.Configuration
{
    [CreateAssetMenu(menuName = "Configuration/Gameplay Configuration")]
    public class GameConfiguration : ScriptableObject
    {
        [SerializeField] private float _asteroidSpawnInterval;
        [SerializeField] private int _maxAsteroidQuantity;
        [SerializeField] private AsteroidGroupConfiguration[] _asteroidGroupConfigurations;

        [SerializeField] private int _ufoSpawnInterval;
        [SerializeField] private int _maxUFOQuantity;
        [SerializeField] private UFOConfiguration _ufoConfiguration;

        [SerializeField] private PlayerConfiguration _playerConfiguration;
        [SerializeField] private BulletConfiguration _playerBulletConfiguration;
        [SerializeField] private LaserConfiguration _laserConfiguration;

        [SerializeField] private int _targetFramerate;

        [SerializeField] private float _worldBoundsPadding;

        public float AsteroidSpawnInterval => _asteroidSpawnInterval;
        public int MaxAsteroidQuantity => _maxAsteroidQuantity;
        public AsteroidGroupConfiguration[] AsteroidGroupConfigurations => _asteroidGroupConfigurations;

        public int UfoSpawnInterval => _ufoSpawnInterval;
        public int MaxUfoQuantity => _maxUFOQuantity;
        public UFOConfiguration UfoConfiguration => _ufoConfiguration;

        public PlayerConfiguration PlayerConfiguration => _playerConfiguration;
        public BulletConfiguration BulletConfiguration => _playerBulletConfiguration;
        public LaserConfiguration LaserConfiguration => _laserConfiguration;

        public float WorldBoundsPadding => _worldBoundsPadding;

        public int TargetFramerate => _targetFramerate;
    }

    public class FieldConfiguration
    {
        public Rect Rect { get; set; }
    }
}
