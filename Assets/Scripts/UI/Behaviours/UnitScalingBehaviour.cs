using System;
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
    public class UnitScalingBehaviour : MonoBehaviour, IInjectable
    {
        [Inject] private readonly World _world;

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
            int entityId = _unitBehaviour.EntityId;
            ref Entity entity = ref _world.GetEntity(entityId);
            Vector3 targetScale = Vector3.one;
            if (entity.HasComponent<ScaleComponent>())
            {
                ref ScaleComponent viewScaleComponent = ref entity.GetComponent<ScaleComponent>();
                targetScale = viewScaleComponent.Scale;
            }

            gameObject.transform.localScale = targetScale;
        }
    }
}
