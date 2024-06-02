using UnityEngine;

namespace Asteroids.Services
{
    [RequireComponent(typeof(Camera))]
    public class GameCamera : MonoBehaviour, IGameWorld
    {
        public Rect WorldRect
        {
            get
            {
                Camera camera = GetComponent<Camera>();
                Rect screenRect = camera.pixelRect;
                Vector3 worldMin = camera.ScreenToWorldPoint(screenRect.min);
                Vector3 worldMax = camera.ScreenToWorldPoint(screenRect.max);
                Vector3 center = (worldMax + worldMin) / 2;
                Vector3 size = worldMax - worldMin;
                Rect worldRect = new Rect(center - size / 2, size);
                return worldRect;
            }
        }
    }
}
