using System;
using System.Collections.Generic;

namespace Asteroids.Tools
{
    public static class DictionaryExtensions
    {
        public static TKey First<TKey, TValue>(this Dictionary<TKey, TValue>.KeyCollection keyCollection)
        {
            using (Dictionary<TKey, TValue>.KeyCollection.Enumerator enumerator = keyCollection.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    return enumerator.Current;
                }
            }

            throw new ArgumentException($"{nameof(Dictionary<TKey, TValue>.KeyCollection)} has no elements");
        }
    }
}
