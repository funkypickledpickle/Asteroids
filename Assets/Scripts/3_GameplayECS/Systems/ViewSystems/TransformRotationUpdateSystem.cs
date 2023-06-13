using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using UnityEngine;

namespace Asteroids.GameplayECS.Systems.ViewSystems
{
    public class TransformRotationUpdateSystem : AbstractExecutableSystem
    {
        protected override EntityGroup CreateContainer()
        {
            return new EntityGroup(World, (ref Entity referenced) => referenced.HasComponent<RotationComponent>() && referenced.HasComponent<ViewComponent>());
        }

        public override void Execute()
        {
            EntityGroup.ForEachOnlyComponents<RotationComponent, ViewComponent>(Execute);
        }

        private static void Execute(ref RotationComponent rotationComponent, ref ViewComponent viewComponent)
        {
            viewComponent.Transform.eulerAngles = new Vector3(0, 0, rotationComponent.RotationDegrees);
        }
    }
}