using System;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Delegates;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;

namespace Asteroids.GameplayECS.Systems.LifeTime
{
    public class DestroyingSystem : IExecutableSystem, IDisposable
    {
        private readonly ValueTypeECS.EntityContainer.World _world;
        private readonly ActionReference<Entity> _executeActionReference;

        private EntityGroup _destroyedEntities;

        public DestroyingSystem(ValueTypeECS.EntityContainer.World world, IInstanceSpawner instanceSpawner)
        {
            _executeActionReference = Execute;

            _world = world;

            _destroyedEntities = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<DestroyedComponent>()
                .Build();
        }

        public void Dispose()
        {
            _destroyedEntities.Dispose();
            _destroyedEntities = null;
        }

        void IExecutableSystem.Execute()
        {
            _destroyedEntities.ForEach(_executeActionReference);
        }

        private void Execute(ref Entity entity)
        {
            _world.RemoveEntity(entity.Id);
        }
    }
}
