using UnityEngine;

namespace Asteroids.Configuration
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

        [SerializeField] private float _gunFiringInterval;
        [SerializeField] private Vector2 _bulletSpawnPositionOffset;

        [SerializeField] private float _laserGunFiringInterval;
        [SerializeField] private Vector2 _laserSpawnPositionOffset;
        [SerializeField] private int _maxChargesQuantity;
        [SerializeField] private int _initialChargesQuantity;
        [SerializeField] private float _laserChargeLoadingDuration;
        [SerializeField] private float _laserDistance;

        public float Mass => _mass;

        public float MaxSpeed => _maxSpeed;
        public float MaxAcceleration => _maxAcceleration;
        public float SpeedStartDumpingFactor => _speedStartDumpingFactor;
        public float SpeedTotalDumpingFactor => _speedTotalDumpingFactor;

        public float MaxAngularSpeed => _maxAngularSpeed;
        public float MaxAngularAcceleration => _maxAngularAcceleration;
        public float AngularSpeedStartDumpingFactor => _angularSpeedStartDumpingFactor;
        public float AngularSpeedTotalDumpingFactor => _angularSpeedTotalDumpingFactor;

        public float GunFiringInterval => _gunFiringInterval;
        public Vector2 BulletSpawnPositionOffset => _bulletSpawnPositionOffset;

        public float LaserGunFiringInterval => _laserGunFiringInterval;
        public Vector2 LaserSpawnPositionOffset => _laserSpawnPositionOffset;
        public int MaxChargesQuantity => _maxChargesQuantity;
        public int InitialChargesQuantity => _initialChargesQuantity;

        public float LaserChargeLoadingDuration => _laserChargeLoadingDuration;
        public float LaserDistance => _laserDistance;
    }
}
