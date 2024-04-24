using System;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.Services;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;

namespace Asteroids.GameplayECS.Systems.LifeTime
{
    public class LifeTimeSystem : IExecutableSystem, IDisposable
    {
        private readonly ActionReferenceValue<Entity, DestroyTimeComponent, float> _executeAction = Execute;

        private readonly IFrameInfoService _frameInfoService;

        private EntityGroup _createdEntities;
        private EntityGroup _initializedEntities;

        public LifeTimeSystem(IFrameInfoService frameInfoService, IInstanceSpawner instanceSpawner)
        {
            _frameInfoService = frameInfoService;
            _createdEntities = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<LifeTimeComponent>()
                .Build();

            _initializedEntities = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<DestroyTimeComponent>()
                .RequireComponentAbsence<DestroyedComponent>()
                .Build();

            _createdEntities.EntityAdded += HandleLifeTimeComponentAdded;
        }

        public void Dispose()
        {
            _createdEntities.EntityAdded -= HandleLifeTimeComponentAdded;
            _createdEntities.Dispose();
            _createdEntities = null;
        }

        private void HandleLifeTimeComponentAdded(ref Entity entity)
        {
            var lifeTime = entity.GetComponent<LifeTimeComponent>().Duration;
            entity.CreateComponent(new DestroyTimeComponent { DeathTime = lifeTime + _frameInfoService.StartTime});
        }

        void IExecutableSystem.Execute()
        {
            _initializedEntities.ForEachComponent(_executeAction, _frameInfoService.StartTime);
        }

        private static void Execute(ref Entity entity, ref DestroyTimeComponent destroyTimeComponent, float frameStartTime)
        {
            if (destroyTimeComponent.DeathTime < frameStartTime)
            {
                entity.CreateComponent<DestroyedComponent>();
            }
        }
    }
}
