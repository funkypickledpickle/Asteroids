using Asteroids.Configuration.Project;
using UnityEngine;

namespace Asteroids.Configuration.Game
{
    [CreateAssetMenu(menuName = "Configuration/Bullet Configuration")]
    public class BulletConfiguration : ScriptableObject
    {
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private ViewKey _viewKey;
        [SerializeField] private float _lifeTime;

        public float BulletSpeed => _bulletSpeed;
        public float LifeTime => _lifeTime;
        public ViewKey ViewKey => _viewKey;
    }
}
