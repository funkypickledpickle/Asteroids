using System;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;
using UnityEngine;

namespace Asteroids.GameplayECS.Systems.PositionSystems
{
    public class ForceResetSystem : IExecutableSystem, IDisposable
    {
        private readonly ActionReference<Entity, UpdatableForceComponent> _executeActionReference = Execute;

        private EntityGroup EntityGroup;

        public ForceResetSystem(IInstanceSpawner instanceSpawner)
        {
            EntityGroup = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<UpdatableForceComponent>()
                .Build();
        }

        public void Dispose()
        {
            EntityGroup.Dispose();
            EntityGroup = null;
        }

        void IExecutableSystem.Execute()
        {
            EntityGroup.ForEachComponent<UpdatableForceComponent>(_executeActionReference);
        }

        private static void Execute(ref Entity entity, ref UpdatableForceComponent forceComponent)
        {
            forceComponent.Force = Vector2.zero;
        }
    }
}
