using System;
using System.Collections.Generic;
using Asteroids.Configuration.Project;
using Asteroids.Tools;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Asteroids.Services
{
    public interface IResourcesService
    {
        T GetAsset<T>(string key) where T : Object;
    }

    public class ResourcesService : IResourcesService
    {
        private readonly Dictionary<string, object> _cachedResources = new Dictionary<string, object>();

        public T GetAsset<T>(string key) where T : Object
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key is null or empty");
            }

            if (_cachedResources.TryGetValue(key, out var resource))
            {
                return (T)resource;
            }

            var resourceType = GetResourceSource(key);
            switch (resourceType)
            {
                case ResourceSource.Local:
                {
                    var assetPath = AssetPath(key);
                    {
                        var asset = Resources.Load<T>(assetPath);
                        if (asset == null)
                        {
                            this.LogWarning(LogCategory.Resources, $"Asset wasn't found by this key {key}");
                        }

                        _cachedResources[key] = asset;
                        return asset;
                    }
                }
                default:
                {
                    throw new ArgumentException($"Unknown source type for key {key}");
                }
            }
        }

        private static ResourceSource GetResourceSource(string key)
        {
            var isLocal = key.IndexOf(ProjectConstants.ResourcePath, StringComparison.Ordinal) == 0;
            if (isLocal)
            {
                return ResourceSource.Local;
            }

            return ResourceSource.Unknown;
        }

        private static string AssetPath(string key)
        {
            var input = key;
            var source = GetResourceSource(input);
            switch (source)
            {
                case ResourceSource.Local:
                {
                    var index = input.IndexOf(ProjectConstants.ResourcePath, StringComparison.Ordinal);
                    return input.Substring(index + ProjectConstants.ResourcePath.Length + 1);
                }
                default:
                {
                    throw new NotImplementedException();
                }
            }
        }

        private enum ResourceSource
        {
            Unknown,
            Local,
        }
    }
}
