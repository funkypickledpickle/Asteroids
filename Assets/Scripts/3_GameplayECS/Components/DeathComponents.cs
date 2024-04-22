using Asteroids.ValueTypeECS.Components;

namespace Asteroids.GameplayECS.Components
{
    public struct LifeTimeComponent : IECSComponent
    {
        public float Duration;
    }

    public struct DestroyTimeComponent : IECSComponent
    {
        public float DeathTime;
    }

    public struct DestroyedComponent : IECSComponent { }
}
