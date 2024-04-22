using System.Collections.Generic;

namespace Asteroids.ValueTypeECS.ECSTypes
{
    public struct ECSTypeKey
    {
        public readonly uint Key;

        public ECSTypeKey(uint key)
        {
            Key = key;
        }

        public static bool operator ==(ECSTypeKey a, ECSTypeKey b)
        {
            return a.Key == b.Key;
        }

        public static bool operator !=(ECSTypeKey a, ECSTypeKey b)
        {
            return a.Key != b.Key;
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public bool Equals(ECSTypeKey other)
        {
            return Key == other.Key;
        }

        public override bool Equals(object obj)
        {
            return obj is ECSTypeKey other && Equals(other);
        }

        public override string ToString()
        {
            return $"TypeKey({Key})";
        }
    }

    public class ECSTypeKeyEqualityComparer : IEqualityComparer<ECSTypeKey>
    {
        public bool Equals(ECSTypeKey x, ECSTypeKey y)
        {
            return x.Key == y.Key;
        }

        public int GetHashCode(ECSTypeKey key)
        {
            return key.Key.GetHashCode();
        }
    }
}
