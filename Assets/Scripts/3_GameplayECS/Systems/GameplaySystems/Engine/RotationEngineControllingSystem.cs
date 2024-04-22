using System;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;

namespace Asteroids.GameplayECS.Systems.Engine
{
    public class RotationEngineControllingSystem : IExecutableSystem, IDisposable
    {
        private EntityGroup _ships;

        public RotationEngineControllingSystem(IInstanceSpawner instanceSpawner)
        {
            _ships = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<MainControlComponent>()
                .RequireComponent<RotationEngineComponent>()
                .Build();
        }

        public void Dispose()
        {
            _ships.Dispose();
            _ships = null;
        }

        void IExecutableSystem.Execute()
        {
            _ships.ForEachComponents<MainControlComponent, RotationEngineComponent>(Execute);
        }

        private static void Execute(ref Entity entity, ref MainControlComponent mainControlComponent, ref RotationEngineComponent rotationEngineComponent)
        {
            rotationEngineComponent.Rotation = mainControlComponent.Rotation;
        }
    }
}
