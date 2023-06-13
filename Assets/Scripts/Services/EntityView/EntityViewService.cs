using System.Collections.Generic;
using Asteroids.Configuration.Project;
using Asteroids.Tools;
using UnityEngine;
using Zenject;

namespace Asteroids.Services.EntityView
{
    public interface IEntityViewService
    {
        GameObject Create(ViewKey viewKey);
        void Remove(ViewKey viewKey, GameObject view);
    }

    public class EntityViewService : IEntityViewService
    {
        [Inject] private IDiContainerWrapper _diContainerWrapper;

        private Dictionary<ViewKey, IMemoryPool<GameObject>> _memoryPools = new Dictionary<ViewKey, IMemoryPool<GameObject>>();

        public GameObject Create(ViewKey viewKey)
        {
            return GetMemoryPool(viewKey).Spawn();
        }

        public void Remove(ViewKey viewKey, GameObject view)
        {
            GetMemoryPool(viewKey).Despawn(view);
        }

        private IMemoryPool<GameObject> GetMemoryPool(ViewKey viewKey)
        {
            if (!_memoryPools.TryGetValue(viewKey, out var memoryPool))
            {
                var childContainer = _diContainerWrapper.CreateChildContainer();
                childContainer.Bind(viewKey);
                childContainer.Bind<IFactory<GameObject>, EntityViewFactory>();
                memoryPool = childContainer.Instantiate<EntityViewMemoryPool>();
                _memoryPools.Add(viewKey, memoryPool);
            }

            return memoryPool;
        }
    }
}
