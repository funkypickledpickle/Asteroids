using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Configuration
{
    [CreateAssetMenu(menuName = "Configuration/ViewPathsContainer")]
    public class ViewPathsContainer : ScriptableObject
    {
        [SerializeField] private List<ViewConfiguration> _configurations;

        public IReadOnlyList<ViewConfiguration> Configurations => _configurations;
    }

    [Serializable]
    public struct ViewConfiguration
    {
        [field: SerializeField] public string Path { get; private set; }
        [field: SerializeField] public uint PreloadedObjectsQuantity { get; private set; }
    }
}
