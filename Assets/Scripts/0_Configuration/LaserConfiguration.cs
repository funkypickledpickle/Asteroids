using UnityEngine;

namespace Asteroids.Configuration
{
    [CreateAssetMenu(menuName = "Configuration/Laser Configuration")]
    public class LaserConfiguration : ScriptableObject
    {
        [SerializeField] private float _lifeTime;

        public float Lifetime => _lifeTime;
    }
}
