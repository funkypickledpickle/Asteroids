using Asteroids.Configuration.Project;
using UnityEngine;

namespace Asteroids.Configuration.Game
{
    [CreateAssetMenu(menuName = "Configuration/Player Configuration")]
    public class PlayerConfiguration : ScriptableObject
    {
        [SerializeField] private float _mass;

        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _maxAcceleration;
        [SerializeField] private float _speedTotalDumpingFactor;
        [SerializeField] private float _speedStartDumpingFactor;

        [SerializeField] private float _maxAngularSpeed;
        [SerializeField] private float _maxAngularAcceleration;
        [SerializeField] private float _angularSpeedStartDumpingFactor;
        [SerializeField] private float _angularSpeedTotalDumpingFactor;

        [SerializeField] private ViewKey _viewKey;

        public float Mass => _mass;

        public float MaxSpeed => _maxSpeed;
        public float MaxAcceleration => _maxAcceleration;
        public float SpeedStartDumpingFactor => _speedStartDumpingFactor;
        public float SpeedTotalDumpingFactor => _speedTotalDumpingFactor;

        public float MaxAngularSpeed => _maxAngularSpeed;
        public float MaxAngularAcceleration => _maxAngularAcceleration;
        public float AngularSpeedStartDumpingFactor => _angularSpeedStartDumpingFactor;
        public float AngularSpeedTotalDumpingFactor => _angularSpeedTotalDumpingFactor;
        public ViewKey ViewKey => _viewKey;
    }
}
