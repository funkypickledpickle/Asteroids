using Asteroids.Configuration;
using Random = UnityEngine.Random;

namespace Asteroids.Tools
{
    public static class MinMaxExtensions
    {
        public static float RandomRange(this MinMax<float> minMax)
        {
            return Random.Range(minMax.Min, minMax.Max);
        }
    }
}
