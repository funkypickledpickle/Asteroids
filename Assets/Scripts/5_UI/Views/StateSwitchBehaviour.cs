using Asteroids.Gameplay.States;
using Asteroids.Installation;
using UnityEngine;
using Zenject;

namespace Asteroids.UI.Views
{
    [RequireComponent(typeof(Injector))]
    public class StateSwitchBehaviour : MonoBehaviour, IInjectable
    {
        [Inject] private readonly IStateContext _stateContext;

        public void OnStartGameStateRequested()
        {
            _stateContext.SwitchState<StartGameState>();
        }

        public void OnGameStateRequested()
        {
            _stateContext.SwitchState<GameState>();
        }

        public void OnPauseStateRequested()
        {
            _stateContext.SwitchState<PauseState>();
        }

        public void OnMenuStateRequested()
        {
            _stateContext.SwitchState<MenuState>();
        }
    }
}
