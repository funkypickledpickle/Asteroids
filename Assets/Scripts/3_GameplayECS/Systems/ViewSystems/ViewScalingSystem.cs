using Asteroids.GameplayECS.Components;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;

namespace Asteroids.GameplayECS.Systems.ViewSystems
{
    public class ViewScalingSystem : AbstractSystem
    {
        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<ViewComponent>()
               .RequireComponent<ViewScaleComponent>()
               .Build();
        }

        protected override void InitializeInternal()
        {
            EntityGroup.SubscribeToEntityAddedEvent(EntityAddedHandler);
        }

        private void EntityAddedHandler(ref Entity referenced)
        {
            ref var viewComponent = ref referenced.GetComponent<ViewComponent>();
            ref var viewScaleComponent = ref referenced.GetComponent<ViewScaleComponent>();
            viewComponent.Transform.localScale = viewScaleComponent.Scale;
        }
    }
}
