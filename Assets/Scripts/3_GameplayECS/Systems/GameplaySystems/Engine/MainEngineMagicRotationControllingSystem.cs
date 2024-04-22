using System;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;

namespace Asteroids.GameplayECS.Systems.Engine
{
    public class MainEngineMagicRotationControllingSystem : IExecutableSystem, IDisposable
    {
        private EntityGroup EntityGroup;

        public MainEngineMagicRotationControllingSystem(IInstanceSpawner instanceSpawner)
        {
            EntityGroup = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<MainControlComponent>()
                .RequireComponent<MainEngineMagicRotationComponent>()
                .Build();
        }

        public void Dispose()
        {
            EntityGroup.Dispose();
            EntityGroup = null;
        }

        void IExecutableSystem.Execute()
        {
            EntityGroup.ForEachComponents<MainControlComponent, MainEngineMagicRotationComponent>(Execute);
        }

        private static void Execute(ref Entity entity, ref MainControlComponent mainControlComponent, ref MainEngineMagicRotationComponent mainEngineMagicRotationComponent)
        {
            mainEngineMagicRotationComponent.Rotation = mainControlComponent.Rotation;
        }
    }
}
