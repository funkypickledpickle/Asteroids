using Asteroids.GameplayECS.Factories;
using Asteroids.Services.EntityView;
using UnityEngine;
using Zenject;

namespace Asteroids.GameplayComponents.Controllers
{
    public class EntityCollisionController : MonoBehaviour, IInjectableController
    {
        [Inject] private readonly EntityFactory _entityFactory;

        [SerializeField] private GameObject _rootGameObject;

        private void OnTriggerEnter2D(Collider2D other)
        {
            _entityFactory.CreateUnityCollision(_rootGameObject == null ? gameObject : _rootGameObject, other.gameObject);
        }
    }
}
