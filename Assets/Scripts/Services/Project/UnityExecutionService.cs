using System;
using UnityEngine;

namespace Asteroids.Services.Project
{
    public interface IFrameInfoService
    {
        public float DeltaTime { get; }
        public float StartTime { get; }
    }

    public interface IUnityExecutionService
    {
        void PauseUpdate();
        void UnPauseUpdate();

        void SubscribeToUpdate(Action action);
        void UnsubscribeFromUpdate(Action action);
    }

    public class UnityExecutionService : MonoBehaviour, IFrameInfoService, IUnityExecutionService
    {
        private const float DefaultTimeScale = 1;
        private const float PausedTimeScale = 0;

        private Action _unityUpdateAction;

        private float _lastStartTime;

        public float DeltaTime { get; private set; }
        public float StartTime { get; private set; }

        public void Update()
        {
            _lastStartTime = StartTime;
            StartTime = Time.time;
            DeltaTime = StartTime - _lastStartTime;

            _unityUpdateAction?.Invoke();
        }

        public void PauseUpdate()
        {
            Time.timeScale = PausedTimeScale;
            gameObject.SetActive(false);
        }

        public void UnPauseUpdate()
        {
            Time.timeScale = DefaultTimeScale;
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
