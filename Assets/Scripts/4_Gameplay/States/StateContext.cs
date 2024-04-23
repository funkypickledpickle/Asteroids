using System;
using System.Collections.Generic;
using Asteroids.Tools;
using Zenject;

namespace Asteroids.Gameplay.States
{
    public interface IStateContext
    {
        public event Action StateChanged;

        public BaseState CurrentState { get; }

        void SwitchState<T>() where T : BaseState;
    }

    public class StateContext : IStateContext
    {
        public event Action StateChanged;

        [Inject] private readonly IInstanceSpawner _instanceSpawner;

        private BaseState _currentState;
        private Dictionary<Type, BaseState> _gameStates = new Dictionary<Type, BaseState>();

        public BaseState CurrentState => _currentState;

        public void SwitchState<T>() where T : BaseState
        {
            if (_currentState != null)
            {
                _currentState.StateEnded();
            }

            _currentState = GetState<T>();
            StateChanged?.Invoke();

            _currentState.StateStarted();
        }

        private BaseState GetState<T>() where T : BaseState
        {
            var type = typeof(T);
            if (_gameStates.TryGetValue(type, out var state))
            {
                return state;
            }

            state = _instanceSpawner.Instantiate<T>();
            _gameStates[type] = state;
            return state;
        }
    }
}
