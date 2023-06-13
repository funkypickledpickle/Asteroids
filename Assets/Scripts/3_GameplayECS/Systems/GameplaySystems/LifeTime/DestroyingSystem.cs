using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.ValueTypeECS.Delegates;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;

namespace Asteroids.GameplayECS.Systems.LifeTime
{
    public class DestroyingSystem : AbstractExecutableSystem
    {
        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<DestroyedComponent>()
               .Build();
        }

        public override void Execute()
        {
            EntityGroup.ForEach(Execute);
        }

        private void Execute(ref Entity entity)
        {
            World.RemoveEntity(entity.Id);
        }
    }
}
