using System;
using UnityEngine;

namespace Asteroids.Services
{
    public interface IExecutionService
    {
        public event Action FrameStarted;

        void PauseExecution();
        void ResumeExecution();
    }

    public interface IFrameInfoService
    {
        public float DeltaTime { get; }
        public float StartTime { get; }
    }

    public class ExecutionService : MonoBehaviour, IFrameInfoService, IExecutionService
    {
        private const float DefaultTimeScale = 1;
        private const float PausedTimeScale = 0;

        public event Action FrameStarted;

        public float DeltaTime { get; private set; }
        public float StartTime { get; private set; }

        private void Update()
        {
            StartTime = Time.time;
            DeltaTime = Time.deltaTime;

            FrameStarted?.Invoke();
        }

        void IExecutionService.PauseExecution()
        {
            Time.timeScale = PausedTimeScale;
            gameObject.SetActive(false);
        }

        void IExecutionService.ResumeExecution()
        {
            Time.timeScale = DefaultTimeScale;
            gameObject.SetActive(true);
        }
    }
}
