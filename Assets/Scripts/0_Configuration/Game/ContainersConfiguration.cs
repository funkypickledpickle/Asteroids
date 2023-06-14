using System;
using UnityEngine;

namespace Asteroids.Configuration.Game
{
    [Serializable]
    public class WorldConfiguration
    {
        [SerializeField] private int _arraySizeForEntitySegmentedList;
        [SerializeField] private int _arraySizeForComponentsSegmentedList;

        public int ArraySizeForEntitySegmentedList => _arraySizeForEntitySegmentedList;
        public int ArraySizeForComponentsSegmentedList => _arraySizeForComponentsSegmentedList;
    }

    [CreateAssetMenu(menuName = "Configuration/Containers Configuration")]
    public class ContainersConfiguration : ScriptableObject
    {
        [SerializeField] private WorldConfiguration _worldConfiguration;
        public WorldConfiguration WorldConfiguration => _worldConfiguration;
    }
}
