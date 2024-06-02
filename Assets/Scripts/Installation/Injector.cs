using Asteroids.UI;
using UnityEngine;
using Zenject;

namespace Asteroids.Installation
{
    [DisallowMultipleComponent]
    public class Injector : MonoBehaviour
    {
        private void Awake()
        {
            DiContainer container = ContainersRegistry.GetContainer(Containers.SceneContainerId);

            foreach (IInjectable injectable in transform.GetComponents<IInjectable>())
            {
                container.Inject(injectable);
            }
        }
    }
}
