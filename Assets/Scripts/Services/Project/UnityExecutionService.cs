using System;
using UnityEngine;

namespace Asteroids.Services.Project
{
    public interface IUnityExecutionService
    {
        void PauseUpdate();
        void UnPauseUpdate();

        void SubscribeToUpdate(Action action);
        void UnsubscribeFromUpdate(Action action);
    }

    public class UnityExecutionService : MonoBehaviour, IUnityExecutionService
    {
        private Action _unityUpdateAction;

        public void Update()
        {
            _unityUpdateAction?.Invoke();
        }

        public void PauseUpdate()
        {
            gameObject.SetActive(false);
        }

        public void UnPauseUpdate()
        {
            gameObject.SetActive(true);
        }

        public void SubscribeToUpdate(Action action)
        {
            _unityUpdateAction += action;
        }

        public void UnsubscribeFromUpdate(Action action)
        {
            _unityUpdateAction -= action;
        }
    }
}
