using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using UnityEngine;

namespace Asteroids.GameplayECS.Systems.AI
{
    public class ShipFollowingSystem : AbstractExecutableSystem
    {
        private EntityGroup _shipGroup;

        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<ShipFollowerComponent>()
               .RequireComponent<MainControlComponent>()
               .RequireComponent<PositionComponent>()
               .Build();
        }

        protected override void InitializeInternal()
        {
            _shipGroup = InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<ShipComponent>()
               .Build();
        }

        public override void Execute()
        {
            EntityGroup.ForEachComponents<MainControlComponent, PositionComponent>(Execute);
        }

        private void Execute(ref Entity entity, ref MainControlComponent mainControlComponent, ref PositionComponent shipPositionComponent)
        {
            if (_shipGroup.Count != 0)
            {
                ref var targetPositionComponent = ref _shipGroup.GetFirst().GetComponent<PositionComponent>();
                var direction = targetPositionComponent.Position - shipPositionComponent.Position;
                mainControlComponent.Acceleration = MainControlComponent.MaxAcceleration;
                mainControlComponent.Rotation = Quaternion.LookRotation(Vector3.forward, direction).eulerAngles.z;
            }
        }
    }
}
