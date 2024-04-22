using System;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;
using UnityEngine;

namespace Asteroids.GameplayECS.Systems.AI
{
    public class ShipFollowingSystem : IExecutableSystem, IDisposable
    {
        private EntityGroup _predators;
        private EntityGroup _ships;

        public ShipFollowingSystem(IInstanceSpawner instanceSpawner)
        {
            _predators = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<ShipFollowerComponent>()
                .RequireComponent<MainControlComponent>()
                .RequireComponent<PositionComponent>()
                .Build();

            _ships = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<ShipComponent>()
                .Build();
        }

        public void Dispose()
        {
            _predators.Dispose();
            _predators = null;

            _ships.Dispose();
            _ships = null;
        }

        void IExecutableSystem.Execute()
        {
            _predators.ForEachComponents<MainControlComponent, PositionComponent>(Execute);
        }

        private void Execute(ref Entity entity, ref MainControlComponent mainControlComponent, ref PositionComponent shipPositionComponent)
        {
            if (_ships.Count == 0)
            {
                return;
            }

            ref var targetPositionComponent = ref _ships.GetFirst().GetComponent<PositionComponent>();
            var direction = targetPositionComponent.Position - shipPositionComponent.Position;
            mainControlComponent.Acceleration = MainControlComponent.MaxAcceleration;
            mainControlComponent.Rotation = Quaternion.LookRotation(Vector3.forward, direction).eulerAngles.z;
        }
    }
}
