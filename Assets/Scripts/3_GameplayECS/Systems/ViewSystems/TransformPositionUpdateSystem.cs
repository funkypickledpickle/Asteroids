using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;

namespace Asteroids.GameplayECS.Systems.ViewSystems
{
    public class TransformPositionUpdateSystem : AbstractExecutableSystem
    {
        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<PositionComponent>()
               .RequireComponent<ViewComponent>()
               .Build();
        }

        public override void Execute()
        {
            EntityGroup.ForEachOnlyComponents<PositionComponent, ViewComponent>(Execute);
        }

        private static void Execute(ref PositionComponent positionComponent, ref ViewComponent viewComponent)
        {
            viewComponent.Transform.position = positionComponent.Position;
        }
    }
}
