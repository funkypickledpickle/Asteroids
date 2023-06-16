using Asteroids.ValueTypeECS.Components;
using UnityEngine;

namespace Asteroids.GameplayECS.Components
{
    public struct LaserGunControlComponent : IECSComponent
    {
        public bool IsFireRequested;
    }

    public struct LaserGunConfigurationComponent
    {
        public float FiringInterval;
        public Vector2 LaserSpawnPositionOffset;
        public float Distance;
    }

    public struct LaserGunComponent : IECSComponent
    {
        public LaserGunConfigurationComponent Configuration;
        public float LastFireTime;
        public bool IsFireRequested;
        public int ChargesCount;
    }

    public struct LaserAutoChargingComponent : IECSComponent
    {
        public LaserAutoChargingConfigurationComponent Configuration;
        public float LastLoadingTime;
        public bool IsAutoCharging;
    }

    public struct LaserAutoChargingConfigurationComponent : IECSComponent
    {
        public float Duration;
        public int MaxChargesQuantity;
    }
}
