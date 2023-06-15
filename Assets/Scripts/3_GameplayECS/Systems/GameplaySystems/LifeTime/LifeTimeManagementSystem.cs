using Asteroids.GameplayECS.Components;
using Asteroids.Services.Project;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Zenject;

namespace Asteroids.GameplayECS.Systems.LifeTime
{
    public class LifeTimeManagementSystem : AbstractSystem
    {
        [Inject] private readonly IFrameInfoService _frameInfoService;

        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<LifeTimeComponent>()
               .Build();
        }

        protected override void InitializeInternal()
        {
            EntityGroup.SubscribeToEntityAddedEvent(EntityAddedHandler);
        }

        private void EntityAddedHandler(ref Entity entity)
        {
            var lifeTime = entity.GetComponent<LifeTimeComponent>().LifeTime;
            entity.CreateComponent(new DeathTimeComponent { DeathTime = lifeTime + _frameInfoService.StartTime});
        }
    }
}
