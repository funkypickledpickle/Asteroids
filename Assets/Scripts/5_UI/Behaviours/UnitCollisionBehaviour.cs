using Asteroids.Installation;
using Asteroids.GameplayECS.Components;
using Asteroids.ValueTypeECS.EntityContainer;
using UnityEngine;
using Zenject;

namespace Asteroids.UI.Behaviours
{
    [RequireComponent(typeof(Injector))]
    [DisallowMultipleComponent]
    public class UnitCollisionBehaviour : MonoBehaviour, IInjectable
    {
        [SerializeField] private UnitBehaviour _unitBehaviour;

        [Inject] private readonly World _world;

        private int _entityId;

        private void Awake()
        {
            _unitBehaviour = GetComponent<UnitBehaviour>();
            _unitBehaviour.WillBeVisible += HandleWillBeVisible;
        }

        private void OnDestroy()
        {
            _unitBehaviour.WillBeVisible -= HandleWillBeVisible;
        }

        private void HandleWillBeVisible()
        {
            _entityId = _unitBehaviour.EntityId;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var clientUnitId = other.GetComponent<UnitBehaviour>();
            if (clientUnitId == null)
            {
                Debug.LogError($"There's no UnitId component in other.gameObject {other.gameObject.name}");
                return;
            }

            ref var collisionHost = ref _world.GetEntity(_entityId);
            if (!collisionHost.HasComponent<CollisionComponent>())
            {
                collisionHost.CreateComponent(new CollisionComponent { EntityId = clientUnitId.EntityId });
            }
        }
    }
}
