using Asteroids.ValueTypeECS.Components;
using UnityEngine;

namespace Asteroids.GameplayECS.Components
{
    public struct ScaleComponent : IECSComponent
    {
        public Vector3 Scale;
    }
}
