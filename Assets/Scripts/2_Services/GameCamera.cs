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
                var camera = GetComponent<Camera>();
                var screenRect = camera.pixelRect;
                var worldMin = camera.ScreenToWorldPoint(screenRect.min);
                var worldMax = camera.ScreenToWorldPoint(screenRect.max);
                var center = (worldMax + worldMin) / 2;
                var size = worldMax - worldMin;
                var worldRect = new Rect(center - size / 2, size);
                return worldRect;
            }
        }
    }
}
