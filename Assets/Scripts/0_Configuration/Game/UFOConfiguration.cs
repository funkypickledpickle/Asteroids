using Asteroids.Configuration.Project;
using UnityEngine;

namespace Asteroids.Configuration.Game
{
    [CreateAssetMenu(menuName = "Configuration/UFO Configuration")]
    public class UFOConfiguration : ScriptableObject
    {
        [SerializeField] private MinMax<float> _speedRange;
        [SerializeField] private float _maxAcceleration;
        [SerializeField] private float _mass;
        [SerializeField] private float _maxDistanceFromTarget;
        [SerializeField] private ViewKey _viewKey;

        public MinMax<float> SpeedRange => _speedRange;
        public float Mass => _mass;
        public float MaxAcceleration => _maxAcceleration;
        public float MaxDistanceFromTarget => _maxDistanceFromTarget;

        public ViewKey ViewKey => _viewKey;
    }

}
