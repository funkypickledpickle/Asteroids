using Asteroids.UI;
using UnityEngine;

namespace Asteroids.Installation
{
    [DisallowMultipleComponent]
    public class Injector : MonoBehaviour
    {
        private void Awake()
        {
            var container = ContainersRegistry.GetContainer(Containers.SceneContainerId);

            foreach (var injectable in transform.GetComponents<IInjectable>())
            {
                container.Inject(injectable);
            }
        }
    }
}
