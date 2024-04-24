using System;
using Asteroids.Gameplay.Controllers;

namespace Asteroids.Gameplay.States
{
    public class InitializationState : BaseState
    {
        private readonly GameController _gameController;

        public InitializationState(GameController gameController, IStateContext stateContext) : base(stateContext)
        {
            _gameController = gameController;
        }

        protected override void StateStartHandler()
        {
            _gameController.Initialize();
            StateContext.SwitchState<MenuState>();
        }
    }

    public class MenuState : BaseState
    {
        public MenuState(IStateContext stateContext) : base(stateContext)
        {
        }
    }

    public class StartGameState : BaseState
    {
        private readonly GameController _gameController;

        public StartGameState(GameController gameController, IStateContext stateContext) : base(stateContext)
        {
            _gameController = gameController;
        }

        protected override void StateStartHandler()
        {
            _gameController.StartGame();
            StateContext.SwitchState<GameState>();
        }
    }

    public class GameState : BaseState
    {
        public GameState(GameController gameController, IStateContext IStateContext) : base(IStateContext)
        {
            gameController.PlayerDestroyed += PlayerDestroyedHandler;
        }

        private void PlayerDestroyedHandler(object sender, EventArgs e)
        {
            StateContext.SwitchState<EndGameState>();
        }
    }

    public class PauseState : BaseState
    {
        private readonly GameController _gameController;

        public PauseState(GameController gameController, IStateContext IStateContext) : base(IStateContext)
        {
            _gameController = gameController;
        }

        protected override void StateStartHandler()
        {
            _gameController.PauseGame();
        }

        protected override void StateEndedHandler()
        {
            _gameController.UnpauseGame();
        }
    }

    public class EndGameState : BaseState
    {
        private readonly GameController _gameController;

        public EndGameState(GameController gameController, IStateContext IStateContext) : base(IStateContext)
        {
            _gameController = gameController;
        }

        protected override void StateStartHandler()
        {
            _gameController.EndGame();
        }

        protected override void StateEndedHandler()
        {
            _gameController.Reset();
        }
    }
}
