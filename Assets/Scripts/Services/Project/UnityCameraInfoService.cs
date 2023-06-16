using UnityEngine;
using Zenject;

namespace Asteroids.Services.Project
{
    public interface ICameraInfoService
    {
        Rect WorldRect { get; }
    }

    public class UnityCameraInfoService : ICameraInfoService
    {
        [Inject] private readonly IUnitySceneService _sceneService;

        public Rect WorldRect
        {
            get
            {
                var camera = _sceneService.Camera;
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
