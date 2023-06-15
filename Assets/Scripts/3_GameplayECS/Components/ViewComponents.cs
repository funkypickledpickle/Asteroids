using Asteroids.Configuration.Project;
using Asteroids.ValueTypeECS.Components;
using UnityEngine;

namespace Asteroids.GameplayECS.Components
{
    public struct ViewKeyComponent : IECSComponent
    {
        public ViewKey ViewKey;
    }

    public struct ViewComponent : IECSComponent
    {
        public GameObject View;
        public Transform Transform;
    }

    public struct ViewCollisionComponent : IECSComponent
    {
        public GameObject Host;
        public GameObject Client;
    }

    public struct ViewCreatedComponent : IECSComponent { }
}
