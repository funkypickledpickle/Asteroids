using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Configuration
{
    [Serializable]
    public struct ViewKey
    {
        [SerializeField] private uint _key;
        public uint Key => _key;

        public ViewKey(uint key)
        {
            _key = key;
        }

        public static bool operator ==(ViewKey a, ViewKey b)
        {
            return a._key == b._key;
        }

        public static bool operator !=(ViewKey a, ViewKey b)
        {
            return a._key != b._key;
        }

        public bool Equals(ViewKey other)
        {
            return _key == other._key;
        }

        public override bool Equals(object obj)
        {
            return obj is ViewKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _key.GetHashCode();
        }
    }

    public class ViewKeyComparer : IEqualityComparer<ViewKey>
    {
        public bool Equals(ViewKey x, ViewKey y)
        {
            return x.Key.Equals(y.Key);
        }

        public int GetHashCode(ViewKey obj)
        {
            return obj.Key.GetHashCode();
        }
    }
}
