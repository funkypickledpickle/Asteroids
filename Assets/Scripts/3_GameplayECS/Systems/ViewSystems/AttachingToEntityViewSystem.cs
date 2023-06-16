using Asteroids.GameplayECS.Components;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using UnityEngine;

namespace Asteroids.GameplayECS.Systems.ViewSystems
{
    public class AttachingToEntityViewSystem : AbstractSystem
    {
        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<AttachedToEntityComponent>()
               .RequireComponent<ViewComponent>()
               .Build();
        }

        protected override void InitializeInternal()
        {
            EntityGroup.SubscribeToEntityAddedEvent(EntityAddedHandler);
        }

        private void EntityAddedHandler(ref Entity entity)
        {
            ref var viewComponent = ref entity.GetComponent<ViewComponent>();
            ref var attachedToEntityComponent = ref entity.GetComponent<AttachedToEntityComponent>();

            ref var parentEntity = ref World.GetEntity(attachedToEntityComponent.EntityId);
            ref var parentViewComponent = ref parentEntity.GetComponent<ViewComponent>();
            var viewTransform = viewComponent.Transform;
            viewTransform.SetParent(parentViewComponent.Transform);
            viewTransform.localPosition = attachedToEntityComponent.PositionOffset;
            viewTransform.localRotation = Quaternion.identity;
        }
    }
}
