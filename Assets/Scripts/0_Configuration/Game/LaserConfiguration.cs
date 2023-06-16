using Asteroids.Configuration.Project;
using UnityEngine;

namespace Asteroids.Configuration.Game
{
    [CreateAssetMenu(menuName = "Configuration/Laser Configuration")]
    public class LaserConfiguration : ScriptableObject
    {
        [SerializeField] private float _lifeTime;
        [SerializeField] private ViewKey _viewKey;

        public float Lifetime => _lifeTime;
        public ViewKey ViewKey => _viewKey;
    }
}
