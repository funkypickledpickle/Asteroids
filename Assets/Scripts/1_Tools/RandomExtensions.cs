using UnityEngine;

namespace Asteroids.Tools
{
    public static class RandomExtensions
    {
        public static int RandomSign()
        {
            return Random.Range(0, 10) >= 5 ? -1 : 1;
        }
    }
}
