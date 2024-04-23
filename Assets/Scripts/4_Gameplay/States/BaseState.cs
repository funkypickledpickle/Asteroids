namespace Asteroids.Gameplay.States
{
    public abstract class BaseState
    {
        protected readonly IStateContext StateContext;
        protected bool IsStateActive { get; private set; }

        public BaseState(IStateContext stateContext)
        {
            StateContext = stateContext;
        }

        public void StateStarted()
        {
            IsStateActive = true;
            StateStartHandler();
            StateSwitchedHandler(true);
        }

        protected virtual void StateStartHandler() { }

        public void StateEnded()
        {
            IsStateActive = false;
            StateEndedHandler();
            StateSwitchedHandler(false);
        }

        protected virtual void StateEndedHandler() { }

        protected virtual void StateSwitchedHandler(bool isStateActive) { }
    }
}
