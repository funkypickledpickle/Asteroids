using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;

namespace Asteroids.GameplayECS.Systems.ViewSystems
{
    public class ViewActivationSystem : AbstractExecutableSystem
    {
        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<ViewComponent>()
               .RequireComponent<ViewCreatedComponent>()
               .Build();
        }

        public override void Execute()
        {
            EntityGroup.ForEach(Execute);
        }

        private static void Execute(ref Entity entity)
        {
            entity.GetComponent<ViewComponent>().View.SetActive(true);
            entity.RemoveComponent<ViewCreatedComponent>();
        }
    }
}
