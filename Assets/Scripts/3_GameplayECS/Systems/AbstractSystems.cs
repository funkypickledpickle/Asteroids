using Asteroids.Tools;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;
using Zenject;

namespace Asteroids.GameplayECS.Systems
{
    public abstract class AbstractSystem : IInitializableSystem
    {
        [Inject] protected readonly ValueTypeECS.EntityContainer.World World;
        [Inject] protected readonly IInstanceSpawner InstanceSpawner;
        protected EntityGroup EntityGroup { get; private set; }

        void IInitializableSystem.Initialize()
        {
            EntityGroup = CreateContainer();
            InitializeInternal();
        }

        protected virtual void InitializeInternal() { }
        protected abstract EntityGroup CreateContainer();
    }

    public abstract class AbstractExecutableSystem : AbstractSystem, IExecutableSystem
    {
        public abstract void Execute();
    }
}
