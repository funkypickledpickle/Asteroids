using Asteroids.ValueTypeECS.Components;
using UnityEngine;

namespace Asteroids.GameplayECS.Components
{
    public struct ViewComponent : IECSComponent
    {
        public GameObject View;
        public Transform Transform;
    }
}
