using UnityEngine;

namespace Asteroids.Configuration
{
    [CreateAssetMenu(menuName = "Configuration/Containers Configuration")]
    public class ContainersConfiguration : ScriptableObject
    {
        [SerializeField] private int _arraySizeForEntitySegmentedList;
        [SerializeField] private int _arraySizeForComponentsSegmentedList;
        [SerializeField] private int _componentsDictionarySizeInEntity;
        [SerializeField] private int _entityGroupIdsMinCapacity;

        public int ArraySizeForEntitySegmentedList => _arraySizeForEntitySegmentedList;
        public int ArraySizeForComponentsSegmentedList => _arraySizeForComponentsSegmentedList;
        public int ComponentsDictionarySizeInEntity => _componentsDictionarySizeInEntity;
        public int EntityGroupIdsMinCapacity => _entityGroupIdsMinCapacity;
    }
}
