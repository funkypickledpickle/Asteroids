using System.Collections.Generic;
using Asteroids.Configuration;
using Asteroids.Configuration.Project;
using UnityEngine;
using UnityEngine.Pool;

namespace Asteroids.Services.EntityView
{
    public interface IEntityViewService
    {
        GameObject Get(ViewKey viewKey);
        void Release(ViewKey viewKey, GameObject view);
    }

    public class EntityViewService : IEntityViewService
    {
        private readonly UnitFactory _unitFactory;

        private Dictionary<ViewKey, IObjectPool<GameObject>> _memoryPools = new Dictionary<ViewKey, IObjectPool<GameObject>>(new ViewKeyComparer());

        public EntityViewService(UnitFactory unitFactory)
        {
            _unitFactory = unitFactory;
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
            if (!_memoryPools.TryGetValue(viewKey, out var memoryPool))
            {
                memoryPool = new ObjectPool<GameObject>(() => _unitFactory.Create(viewKey),
                    gameObject => gameObject.SetActive(false), gameObject => gameObject.SetActive(false));
                _memoryPools.Add(viewKey, memoryPool);
            }

            return memoryPool;
        }
    }

}
