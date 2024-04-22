using UnityEngine;

namespace Asteroids.Configuration
{
    [CreateAssetMenu(menuName = "Configuration/Bullet Configuration")]
    public class BulletConfiguration : ScriptableObject
    {
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private float _lifeTime;

        public float BulletSpeed => _bulletSpeed;
        public float LifeTime => _lifeTime;
    }
}
