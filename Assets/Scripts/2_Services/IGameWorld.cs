using UnityEngine;

namespace Asteroids.Services
{
    public interface IGameWorld
    {
        Rect WorldRect { get; }
    }
}
