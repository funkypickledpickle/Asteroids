using System.Collections.Generic;
using Asteroids.Configuration;
using Asteroids.Services.Project;
using UnityEngine;
using UnityEngine.Pool;

namespace Asteroids.Services.EntityView
{
    public interface IEntityViewService
    {
        void Preload();
        GameObject Get(ViewKey viewKey);
        void Release(ViewKey viewKey, GameObject view);
    }

    public class EntityViewService : IEntityViewService
    {
        private readonly UnitFactory _unitFactory;
        private readonly IViewConfigurationService _viewConfigurationService;

        private Dictionary<ViewKey, IObjectPool<GameObject>> _memoryPools = new Dictionary<ViewKey, IObjectPool<GameObject>>(new ViewKeyComparer());

        public EntityViewService(UnitFactory unitFactory, IViewConfigurationService viewConfigurationService)
        {
            _unitFactory = unitFactory;
            _viewConfigurationService = viewConfigurationService;
        }

        public void Preload()
        {
            List<GameObject> createdGameObjects = new List<GameObject>();
            IEnumerable<ViewPreloadConfiguration> configuration = _viewConfigurationService.GetPreloadConfiguration();
            foreach (ViewPreloadConfiguration preloadConfiguration in configuration)
            {
                IObjectPool<GameObject> pool = GetMemoryPool(preloadConfiguration.Key);

                for (int i = 0; i < preloadConfiguration.PreloadedObjectsQuantity; i++)
                {
                    createdGameObjects.Add(pool.Get());
                }
                for (int i = 0; i < createdGameObjects.Count; i++)
                {
                    pool.Release(createdGameObjects[i]);
                }

                createdGameObjects.Clear();
            }
        }

        public GameObject Get(ViewKey viewKey)
        {
            return GetMemoryPool(viewKey).Get();
        }

        public void Release(ViewKey viewKey, GameObject view)
        {
            GetMemoryPool(viewKey).Release(view);
        }

        private IObjectPool<GameObject> GetMemoryPool(ViewKey viewKey)
        {
            if (!_memoryPools.TryGetValue(viewKey, out IObjectPool<GameObject> memoryPool))
            {
                memoryPool = CreateMemoryPool(viewKey);
                _memoryPools.Add(viewKey, memoryPool);
            }

            return memoryPool;
        }

        private IObjectPool<GameObject> CreateMemoryPool(ViewKey viewKey)
        {
            return new ObjectPool<GameObject>(() => _unitFactory.Create(viewKey),
                gameObject => gameObject.SetActive(false), gameObject => gameObject.SetActive(false));
        }
    }

}
