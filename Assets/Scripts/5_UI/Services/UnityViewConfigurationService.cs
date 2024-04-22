using System.Collections.Generic;
using System.Linq;
using Asteroids.Configuration;

namespace Asteroids.Services.Project
{
    public interface IViewConfigurationService
    {
        string GetPath(ViewKey viewKey);
    }

    public class UnityViewConfigurationService : IViewConfigurationService
    {
        private Dictionary<ViewKey, string> _paths;

        public UnityViewConfigurationService(ViewPathsContainer viewPathsContainer)
        {
            _paths = viewPathsContainer.Paths.Select((e, i) => (e, i)).ToDictionary(key => new ViewKey((uint)key.i), value => value.e);
        }

        public string GetPath(ViewKey viewKey)
        {
            return _paths[viewKey];
        }
    }
}
