using Asteroids.Configuration.Project;
using UnityEngine;

namespace Asteroids.Configuration.Game
{
    [CreateAssetMenu(menuName = "Configuration/Player Configuration")]
    public class PlayerConfiguration : ScriptableObject
    {
        [SerializeField] private float _mass;
        [SerializeField] private float _maxAcceleration;
        [SerializeField] private float _maxAngularAcceleration;

        [SerializeField] private ViewKey _viewKey;

        public float Mass => _mass;
        public float MaxAcceleration => _maxAcceleration;
        public float MaxAngularAcceleration => _maxAngularAcceleration;

        public ViewKey ViewKey => _viewKey;
    }
}
