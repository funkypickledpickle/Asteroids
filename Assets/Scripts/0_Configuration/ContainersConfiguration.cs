using UnityEngine;

namespace Asteroids.Configuration
{
    [CreateAssetMenu(menuName = "Configuration/Containers Configuration")]
    public class ContainersConfiguration : ScriptableObject
    {
        [SerializeField] private int _arraySizeForEntitySegmentedList;
        [SerializeField] private int _arraySizeForComponentsSegmentedList;

        public int ArraySizeForEntitySegmentedList => _arraySizeForEntitySegmentedList;
        public int ArraySizeForComponentsSegmentedList => _arraySizeForComponentsSegmentedList;
    }
}
