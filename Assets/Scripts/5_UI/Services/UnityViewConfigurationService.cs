using System.Collections.Generic;
using System.Linq;
using Asteroids.Configuration;

namespace Asteroids.Services.Project
{
    public interface IViewConfigurationService
    {
        string GetPath(ViewKey viewKey);
        IEnumerable<ViewPreloadConfiguration> GetPreloadConfiguration();
    }

    public class UnityViewConfigurationService : IViewConfigurationService
    {
        private readonly IReadOnlyList<ViewConfiguration> _configurations;
        private Dictionary<ViewKey, string> _paths;

        public UnityViewConfigurationService(ViewPathsContainer viewPathsContainer)
        {
            _configurations = viewPathsContainer.Configurations;
            _paths = _configurations.Select((e, i) => (e.Path, i)).ToDictionary(key => new ViewKey((uint)key.i), value => value.Path);
        }

        public string GetPath(ViewKey viewKey)
        {
            return _paths[viewKey];
        }

        public IEnumerable<ViewPreloadConfiguration> GetPreloadConfiguration()
        {
            for (int i = 0; i < _configurations.Count; i++)
            {
                yield return new ViewPreloadConfiguration(new ViewKey((uint)i), _configurations[i].PreloadedObjectsQuantity);
            }
        }
    }

    public struct ViewPreloadConfiguration
    {
        public readonly ViewKey Key;
        public readonly uint PreloadedObjectsQuantity;

        public ViewPreloadConfiguration(ViewKey key, uint preloadedObjectsQuantity)
        {
            Key = key;
            PreloadedObjectsQuantity = preloadedObjectsQuantity;
        }
    }
}
