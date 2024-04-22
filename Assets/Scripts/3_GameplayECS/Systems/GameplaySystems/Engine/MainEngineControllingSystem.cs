using System;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;

namespace Asteroids.GameplayECS.Systems.Engine
{
    public class MainEngineControllingSystem : IExecutableSystem, IDisposable
    {
        private EntityGroup _ships;

        public MainEngineControllingSystem(IInstanceSpawner instanceSpawner)
        {
            _ships = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<MainControlComponent>()
                .RequireComponent<MainEngineComponent>()
                .Build();
        }

        public void Dispose()
        {
            _ships.Dispose();
            _ships = null;
        }

        void IExecutableSystem.Execute()
        {
            _ships.ForEachComponents<MainControlComponent, MainEngineComponent>(Execute);
        }

        private static void Execute(ref Entity entity, ref MainControlComponent mainControlComponent, ref MainEngineComponent mainEngineComponent)
        {
            mainEngineComponent.Acceleration = mainControlComponent.Acceleration;
        }
    }
}
