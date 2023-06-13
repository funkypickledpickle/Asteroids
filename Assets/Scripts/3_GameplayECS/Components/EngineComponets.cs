using Asteroids.ValueTypeECS.Components;

namespace Asteroids.GameplayECS.Components
{
    public struct MainControlComponent : IECSComponent
    {
        public float Rotation;
        public float Acceleration;
    }

    public struct MainEngineConfigurationComponent : IECSComponent
    {
        public float MaxForce;
    }

    public struct MainEngineComponent : IECSComponent
    {
        public bool IsActive => Acceleration != 0;
        public float Acceleration;
    }

    public struct RotationEngineConfigurationComponent : IECSComponent
    {
        public float MaxAngularForce;
    }

    public struct RotationEngineComponent : IECSComponent
    {
        public bool IsActive => Rotation != 0;
        public float Rotation;
    }
}
