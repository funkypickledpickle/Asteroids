using System;
using UnityEngine;

namespace Asteroids.Configuration.Game
{
    [Serializable]
    public struct MinMax<TValue> where TValue : struct
    {
        [SerializeField] private TValue _min;
        [SerializeField] private TValue _max;

        public TValue Min => _min;
        public TValue Max => _max;
    }
}
