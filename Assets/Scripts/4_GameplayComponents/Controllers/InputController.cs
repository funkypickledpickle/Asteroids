using Asteroids.GameplayComponents.Generated;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.Services.Project;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.EntityGroup;
using Zenject;

namespace Asteroids.GameplayComponents.Controllers
{
    public class InputController
    {
        [Inject] private readonly IUnityExecutionService _executionService;

        private EntityGroup _controlledGroup;
        private GameplayInputCollection _gameplayInputCollection;

        public InputController(IInstanceSpawner instanceSpawner, GameplayInputCollection gameplayInputCollection)
        {
            _controlledGroup = instanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<MainControlComponent>()
               .RequireComponent<GunControlComponent>()
               .Build();

            _gameplayInputCollection = gameplayInputCollection;
            _gameplayInputCollection.Enable();
        }

        public void Enable()
        {
            _executionService.SubscribeToUpdate(Execute);
        }

        public void Disable()
        {
            _executionService.UnsubscribeFromUpdate(Execute);
        }

        private void Execute()
        {
            if (_controlledGroup.Count != 0)
            {
                _controlledGroup.GetFirst().GetComponent<MainControlComponent>().Acceleration = _gameplayInputCollection.Gameplay.Acceleration.ReadValue<float>();
                _controlledGroup.GetFirst().GetComponent<MainControlComponent>().Rotation = -_gameplayInputCollection.Gameplay.Rotation.ReadValue<float>();
                _controlledGroup.GetFirst().GetComponent<GunControlComponent>().IsFireRequested = _gameplayInputCollection.Gameplay.Fire.IsPressed();
            }
        }
    }
}
