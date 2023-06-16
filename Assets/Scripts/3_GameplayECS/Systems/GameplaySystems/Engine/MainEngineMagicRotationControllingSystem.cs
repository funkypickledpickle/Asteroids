using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;

namespace Asteroids.GameplayECS.Systems.Engine
{
    public class MainEngineMagicRotationControllingSystem : AbstractExecutableSystem
    {
        private readonly ActionReference<Entity, MainControlComponent, MainEngineMagicRotationComponent> _executeActionReference = Execute;

        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<MainControlComponent>()
               .RequireComponent<MainEngineMagicRotationComponent>()
               .Build();
        }

        public override void Execute()
        {
            EntityGroup.ForEachComponents(_executeActionReference);
        }

        private static void Execute(ref Entity entity, ref MainControlComponent mainControlComponent, ref MainEngineMagicRotationComponent mainEngineMagicRotationComponent)
        {
            mainEngineMagicRotationComponent.Rotation = mainControlComponent.Rotation;
        }
    }
}
