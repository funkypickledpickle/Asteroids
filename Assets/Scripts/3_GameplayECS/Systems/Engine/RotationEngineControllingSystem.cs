using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;

namespace Asteroids.GameplayECS.Systems.Engine
{
    public class RotationEngineControllingSystem : AbstractExecutableSystem
    {
        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<MainControlComponent>()
               .RequireComponent<RotationEngineComponent>()
               .Build();
        }

        public override void Execute()
        {
            EntityGroup.ForEachComponents<MainControlComponent, RotationEngineComponent>(Execute);
        }

        private static void Execute(ref Entity entity, ref MainControlComponent mainControlComponent, ref RotationEngineComponent rotationEngineComponent)
        {
            rotationEngineComponent.Rotation = mainControlComponent.Rotation;
        }
    }
}
