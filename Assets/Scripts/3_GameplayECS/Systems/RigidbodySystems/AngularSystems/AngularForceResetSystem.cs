using System;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;

namespace Asteroids.GameplayECS.Systems.AngularSystems
{
    public class AngularForceResetSystem : IExecutableSystem, IDisposable
    {
        private readonly ActionReference<Entity, UpdatableAngularForceComponent> _executeActionReference = Execute;

        private EntityGroup _ships;

        public AngularForceResetSystem(IInstanceSpawner instanceSpawner)
        {
            _ships = instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<UpdatableAngularForceComponent>()
                .Build();
        }

        public void Dispose()
        {
            _ships.Dispose();
            _ships = null;
        }

        void IExecutableSystem.Execute()
        {
            _ships.ForEachComponent(_executeActionReference);
        }

        private static void Execute(ref Entity entity, ref UpdatableAngularForceComponent forceComponent)
        {
            forceComponent.AngularForce = 0;
        }
    }
}
