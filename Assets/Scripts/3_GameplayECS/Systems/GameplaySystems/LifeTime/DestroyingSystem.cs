using System;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;

namespace Asteroids.GameplayECS.Systems.LifeTime
{
    public class DestroyingSystem : IExecutableSystem, IDisposable
    {
        private readonly ValueTypeECS.EntityContainer.World _world;

        private EntityGroup _destroyedEntities;

        public DestroyingSystem(ValueTypeECS.EntityContainer.World world, IInstanceSpawner instanceSpawner)
        {
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
            _destroyedEntities.ForEach(Execute);
        }

        private void Execute(ref Entity entity)
        {
            _world.RemoveEntity(entity.Id);
        }
    }
}
