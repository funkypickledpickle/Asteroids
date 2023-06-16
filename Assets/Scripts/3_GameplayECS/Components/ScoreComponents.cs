using Asteroids.ValueTypeECS.Components;

namespace Asteroids.GameplayECS.Components
{
    public struct ScoreComponent : IECSComponent
    {
        public int Score;
    }

    public struct ReceivedScoreComponent : IECSComponent
    {
        public int Score;
    }

    public struct RewardableScoreComponent : IECSComponent
    {
        public int Score;
    }
}
