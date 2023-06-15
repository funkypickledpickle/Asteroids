using Asteroids.ValueTypeECS.Components;

namespace Asteroids.GameplayECS.Components
{
    public struct LifeTimeComponent : IECSComponent
    {
        public float LifeTime;
    }

    public struct DeathTimeComponent : IECSComponent
    {
        public float DeathTime;
    }

    public struct DestroyedComponent : IECSComponent { }
}
