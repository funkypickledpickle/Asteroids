using System;
using System.Collections.Generic;
using Asteroids.Gameplay.States;
using Asteroids.Installation;
using UnityEngine;
using Zenject;

namespace Asteroids.UI.Views
{
    [RequireComponent(typeof(Injector))]
    public class Window : MonoBehaviour, IInjectable
    {
        [Inject] private readonly IStateContext _gameStateContext;

        [SerializeField] private GameObject _gameStateUI;
        [SerializeField] private GameObject _pauseStateUI;
        [SerializeField] private GameObject _menuStateUI;
        [SerializeField] private GameObject _endGameStateUI;

        private Dictionary<Type, GameObject> _ui;

        private GameObject _currentActiveUI;

        private void Awake()
        {
            _ui = new Dictionary<Type, GameObject>
            {
                { typeof(GameState), _gameStateUI },
                { typeof(PauseState), _pauseStateUI },
                { typeof(MenuState), _menuStateUI },
                { typeof(EndGameState), _endGameStateUI }
            };
        }

        private void OnEnable()
        {
            _gameStateContext.StateChanged += HandleStateChanged;
            HandleStateChanged();
        }

        private void OnDisable()
        {
            _gameStateContext.StateChanged -= HandleStateChanged;
        }

        private void HandleStateChanged()
        {
            _ui.TryGetValue(_gameStateContext.CurrentState?.GetType(), out var targetUI);
            SwitchToUI(targetUI);
        }

        private void SwitchToUI(GameObject ui)
        {
            if (_currentActiveUI != null)
            {
                _currentActiveUI.SetActive(false);
            }

            _currentActiveUI = ui;

            if (ui != null)
            {
                ui.SetActive(true);
            }
        }
    }
}
