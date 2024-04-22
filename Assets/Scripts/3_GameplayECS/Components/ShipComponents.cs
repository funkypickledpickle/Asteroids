using Asteroids.ValueTypeECS.Components;

namespace Asteroids.GameplayECS.Components
{
    public struct MainControlComponent : IECSComponent
    {
        public const float MaxAcceleration = 1;

        public float Rotation;
        public float Acceleration;
    }

    public struct ShipComponent : IECSComponent { }
}
