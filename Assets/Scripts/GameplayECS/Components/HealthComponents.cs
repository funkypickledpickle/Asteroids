using Asteroids.ValueTypeECS.Components;

namespace Asteroids.GameplayECS.Components
{
    public struct ReceivedDamageComponent : IECSComponent
    {
        public int SourceEntityId;
    }
}
