using Asteroids.Installation;
using Asteroids.GameplayECS.Components;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityContainer;
using UnityEngine;
using Zenject;

namespace Asteroids.UI.Behaviours
{
    [RequireComponent(typeof(Injector), typeof(UnitBehaviour))]
    [DisallowMultipleComponent]
    public class UnitMovementBehaviour : MonoBehaviour, IInjectable
    {
        [Inject] private readonly World _world;

        private int _entityId;
        private UnitBehaviour _unitBehaviour;

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
            Update();
        }

        private void Update()
        {
            ref Entity entity = ref _world.GetEntity(_entityId);
            Transform entityTransform = transform;
            entityTransform.position = entity.GetComponent<PositionComponent>().Position;
            entityTransform.eulerAngles = new Vector3(0, 0, entity.GetComponent<RotationComponent>().RotationDegrees);
        }
    }
}
