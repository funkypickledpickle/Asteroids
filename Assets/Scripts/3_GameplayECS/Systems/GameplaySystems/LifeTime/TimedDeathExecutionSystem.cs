using System;
using Asteroids.GameplayECS.Components;
using Asteroids.Services.Project;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Zenject;

namespace Asteroids.GameplayECS.Systems.LifeTime
{
    public class TimedDeathExecutionSystem : AbstractSystem
    {
        [Inject] private readonly IActionSchedulingService _actionSchedulingService;
        [Inject] private readonly IFrameInfoService _frameInfoService;

        private readonly Action _destroyEntityAction;

        public TimedDeathExecutionSystem()
        {
            _destroyEntityAction = DestroyEntity;
        }

        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<DeathTimeComponent>()
               .Build();
        }

        protected override void InitializeInternal()
        {
            EntityGroup.SubscribeToEntityAddedEvent(EntityAddedHandler);
        }

        private void EntityAddedHandler(ref Entity referenced)
        {
            _actionSchedulingService.Schedule(referenced.GetComponent<DeathTimeComponent>().DeathTime, _destroyEntityAction);
        }

        private void DestroyEntity()
        {
            var frameStartTime = _frameInfoService.StartTime;
            foreach (var entityId in EntityGroup)
            {
                ref var entity = ref World.GetEntity(entityId);
                if (!entity.HasComponent<DestroyedComponent>() && entity.GetComponent<DeathTimeComponent>().DeathTime < frameStartTime)
                {
                    entity.CreateComponent<DestroyedComponent>();
                    return;
                }
            }
        }
    }
}
