using System.Collections.Generic;
using Zenject;

namespace Asteroids.Installation
{
    public static class ContainersRegistry
    {
        private static readonly Dictionary<string, DiContainer> _containers = new Dictionary<string, DiContainer>();

        public static void AddContainer(string id, DiContainer container)
        {
            _containers.Add(id, container);
        }

        public static DiContainer GetContainer(string id)
        {
            if (_containers.TryGetValue(id, out var container))
            {
                return container;
            }

            return null;
        }

        public static void RemoveContainer(string id)
        {
            _containers.Remove(id);
        }
    }

    public static class Containers
    {
        public const string SceneContainerId = "Scene";
    }
}
