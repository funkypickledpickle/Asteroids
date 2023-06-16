using Asteroids.ValueTypeECS.Components;
using UnityEngine;

namespace Asteroids.GameplayECS.Components
{
    public struct GameComponent : IECSComponent { }

    public struct WorldBoundsComponent : IECSComponent
    {
        public Rect Bounds;
    }
}
