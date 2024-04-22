using Asteroids.ValueTypeECS.Components;

namespace Asteroids.GameplayECS.Components
{
    public struct AsteroidComponent : IECSComponent
    {
        public int GroupConfigurationIndex;
        public int StateIndex;
    }

    public struct MeteoriteComponent : IECSComponent { }

    public struct AsteroidSplitComponent : IECSComponent { }

    public struct AsteroidSpawningTimerComponent : IECSComponent { }
}
