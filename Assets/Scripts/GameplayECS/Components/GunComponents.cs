using UnityEngine;
using Asteroids.ValueTypeECS.Components;

namespace Asteroids.GameplayECS.Components
{
    public struct GunControlComponent : IECSComponent
    {
        public bool IsFireRequested;
    }

    public struct GunConfigurationComponent
    {
        public float FiringInterval;
        public float BulletSpeed;
        public Vector2 BulletSpawnPositionOffset;
    }

    public struct GunComponent : IECSComponent
    {
        public GunConfigurationComponent Configuration;
        public float LastFireTime;
        public bool IsFireRequested;
    }
}
