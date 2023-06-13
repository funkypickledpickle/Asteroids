using UnityEngine;
using Asteroids.ValueTypeECS.Components;

namespace Asteroids.GameplayECS.Components
{
    public struct PositionComponent : IECSComponent
    {
        public Vector2 Position;
    }

    public struct RotationComponent : IECSComponent
    {
        public float RotationDegrees;
    }
}
