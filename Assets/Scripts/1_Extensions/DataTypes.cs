using Asteroids.Configuration.Game;
using Random = UnityEngine.Random;

namespace Asteroids.Extensions
{
    public static class MinMaxExtensions
    {
        public static float RandomRange(this MinMax<float> minMax)
        {
            return Random.Range(minMax.Min, minMax.Max);
        }

        public static float RandomRange(this MinMax<int> minMax)
        {
            return Random.Range(minMax.Min, minMax.Max);
        }
    }
}
