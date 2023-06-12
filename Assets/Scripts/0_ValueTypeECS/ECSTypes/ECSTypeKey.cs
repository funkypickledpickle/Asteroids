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
