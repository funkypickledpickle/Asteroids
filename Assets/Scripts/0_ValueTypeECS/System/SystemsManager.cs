using System;
using System.Collections.Generic;
using System.Linq;
using Asteroids.Tools;
using Zenject;

namespace Asteroids.ValueTypeECS.System
{
    public class SystemsManager
    {
        [Inject] private readonly IDiContainerWrapper _instanceSpawner;

        private readonly List<ISystem> _systems = new List<ISystem>();
        private readonly List<IExecutableSystem> _executableSystems = new List<IExecutableSystem>();

        public void AddSystem<TSystem>() where TSystem : class, ISystem
        {
            var system = _instanceSpawner.Instantiate<TSystem>();
            _systems.Add(system);
            if (system is IExecutableSystem executableSystem)
            {
                _executableSystems.Add(executableSystem);
            }
        }

        public void Initialize()
        {
            foreach (var system in _systems.OfType<IInitializableSystem>())
            {
                system.Initialize();
            }
        }

        public void Execute()
        {
            foreach (var system in _executableSystems)
            {
                system.Execute();
            }
        }
    }
}
