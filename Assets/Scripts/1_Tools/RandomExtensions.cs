using UnityEngine;

namespace Asteroids.Extensions
{
    public static class RandomExtensions
    {
        public static int RandomSign()
        {
            return Random.Range(0, 10) >= 5 ? -1 : 1;
        }

        public static bool RandomBool()
        {
            return RandomSign() == 1;
        }
    }
}
