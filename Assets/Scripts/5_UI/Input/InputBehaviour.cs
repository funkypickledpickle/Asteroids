using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.Installation;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.EntityGroup;
using UnityEngine;
using Zenject;

namespace Asteroids.UI.Input
{
    [RequireComponent(typeof(Injector))]
    public class InputBehaviour : MonoBehaviour, IInjectable
    {
        [Inject] private readonly IInstanceSpawner _instanceSpawner;
        [Inject] private readonly GameplayInputCollection _gameplayInputCollection;

        private EntityGroup _controlledGroup;

        private void Awake()
        {
            _controlledGroup = _instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<MainControlComponent>()
                .RequireComponent<GunControlComponent>()
                .RequireComponent<LaserGunControlComponent>()
                .Build();

            _gameplayInputCollection.Enable();
        }

        private void OnDestroy()
        {
            _controlledGroup.Dispose();
            _controlledGroup = null;

            _gameplayInputCollection.Disable();
        }

        private void Update()
        {
            if (_controlledGroup.Count != 0)
            {
                ref var mainControlComponent = ref _controlledGroup.GetFirst().GetComponent<MainControlComponent>();
                mainControlComponent.Acceleration = _gameplayInputCollection.Gameplay.Acceleration.ReadValue<float>();
                mainControlComponent.Rotation = -_gameplayInputCollection.Gameplay.Rotation.ReadValue<float>();
                _controlledGroup.GetFirst().GetComponent<GunControlComponent>().IsFireRequested = _gameplayInputCollection.Gameplay.Fire.IsPressed();
                _controlledGroup.GetFirst().GetComponent<LaserGunControlComponent>().IsFireRequested = _gameplayInputCollection.Gameplay.SecondaryFire.IsPressed();
            }
        }
    }
}
