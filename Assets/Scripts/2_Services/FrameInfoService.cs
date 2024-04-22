using UnityEngine;

namespace Asteroids.Services
{
    public interface IFrameInfoService
    {
        public float DeltaTime { get; }
        public float StartTime { get; }
    }

    public class FrameInfoService : MonoBehaviour, IFrameInfoService
    {
        public float DeltaTime { get; private set; }
        public float StartTime { get; private set; }

        private void Update()
        {
            StartTime = Time.time;
            DeltaTime = Time.deltaTime;
        }
    }
}
