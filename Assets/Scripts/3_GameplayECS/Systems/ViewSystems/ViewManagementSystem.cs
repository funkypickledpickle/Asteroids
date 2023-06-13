using Asteroids.GameplayECS.Components;
using Asteroids.Services.EntityView;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;
using Zenject;

namespace Asteroids.GameplayECS.Systems.ViewSystems
{
    public class ViewManagementSystem : AbstractSystem
    {
        [Inject] private readonly IEntityViewService _entityViewService;

        private EntityGroup _destroyedEntities;

        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<ViewKeyComponent>()
               .Build();
        }

        protected override void InitializeInternal()
        {
            _destroyedEntities = InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<ViewKeyComponent>()
               .RequireComponent<ViewComponent>()
               .RequireComponent<DestroyedComponent>()
               .Build();

            EntityGroup.SubscribeToEntityAddedEvent(EntityAdded);
            _destroyedEntities.SubscribeToEntityAddedEvent(EntityDestroyed);
        }

        private void EntityAdded(ref Entity entity)
        {
            var viewKey = entity.GetComponent<ViewKeyComponent>().ViewKey;
            var view = _entityViewService.Create(viewKey);
            entity.CreateComponent(new ViewComponent
            {
                View = view,
                Transform = view.transform,
            });
            entity.CreateComponent<ViewCreatedComponent>();
        }

        private void EntityDestroyed(ref Entity entity)
        {
            ref var viewComponent = ref entity.GetComponent<ViewComponent>();
            ref var viewKeyComponent = ref entity.GetComponent<ViewKeyComponent>();
            _entityViewService.Remove(viewKeyComponent.ViewKey, viewComponent.View);
            entity.RemoveComponent<ViewComponent>();
        }
    }
}
