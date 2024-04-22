using System;
using System.Collections.Generic;
using System.Linq;

namespace Asteroids.Services.Project
{
    public interface IConfigurationService
    {
        TConfiguration Get<TConfiguration>() where TConfiguration : class;
    }

    public class ScriptableObjectConfigurationService : IConfigurationService
    {
        private Dictionary<Type, object> _configurations;

        public ScriptableObjectConfigurationService(IUnitySceneService sceneService)
        {
            _configurations = sceneService.ConfigurationContainer.Configurations.ToDictionary(key => key.GetType(), value => (object)value);
        }

        public TConfiguration Get<TConfiguration>() where TConfiguration : class
        {
            var key = typeof(TConfiguration);
            if (!_configurations.TryGetValue(key, out var configuration))
            {
                throw new ArgumentException($"Unable to find configuration of type {key}");
            }

            return configuration as TConfiguration;
        }
    }
}
