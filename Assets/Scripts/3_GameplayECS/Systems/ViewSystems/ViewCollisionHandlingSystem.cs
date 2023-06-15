using System;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Containers;
using Asteroids.GameplayECS.Extensions;
using Asteroids.Services.EntityView;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Zenject;

namespace Asteroids.GameplayECS.Systems.ViewSystems
{
    public class ViewCollisionHandlingSystem : AbstractExecutableSystem
    {
        [Inject] private readonly IEntityViewContainer _entityViewContainer;

        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<ViewCollisionComponent>()
               .Build();
        }

        public override void Execute()
        {
            EntityGroup.ForEachComponent<ViewCollisionComponent>(Execute);
        }

        private void Execute(ref Entity entity, ref ViewCollisionComponent component)
        {
            var collisionHostId = _entityViewContainer.GetEntityId(component.Host);
            var collisionClientId = _entityViewContainer.GetEntityId(component.Client);
            if (collisionHostId == null)
            {
                entity.CreateComponent<DestroyedComponent>();
                return;
            }

            if (collisionClientId == null)
            {
                throw new InvalidOperationException($"Collision client was removed. UnityCollision Entity : {entity.Id} The host is {collisionHostId}");
            }

            ref var collisionHost = ref World.GetEntity(collisionHostId.Value);
            if (!collisionHost.HasComponent<CollisionComponent>())
            {
                collisionHost.CreateComponent(new CollisionComponent { EntityId = collisionClientId.Value });
                entity.CreateComponent<DestroyedComponent>();
            }
        }
    }
}
