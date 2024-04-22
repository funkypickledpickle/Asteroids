using UnityEngine;

namespace Asteroids.Configuration
{
    [CreateAssetMenu(menuName = "Configuration/Asteroid Configuration")]
    public class AsteroidConfiguration : ScriptableObject
    {
        [SerializeField] private MinMax<float> _minMaxdistanceFromTarget;
        [SerializeField] private MinMax<float> _minMaxDiversionFromTargetDegrees;
        [SerializeField] private MinMax<float> _minMaxVelocity;
        [SerializeField] private MinMax<float> _minMaxAngularSpeedDegrees;

        public MinMax<float> MinMaxDistanceFromTarget => _minMaxdistanceFromTarget;
        public MinMax<float> MinMaxDiversionFromTargetDegrees => _minMaxDiversionFromTargetDegrees;
        public MinMax<float> MinMaxVelocity => _minMaxVelocity;
        public MinMax<float> MinMaxAngularSpeedDegrees => _minMaxAngularSpeedDegrees;
    }
}
