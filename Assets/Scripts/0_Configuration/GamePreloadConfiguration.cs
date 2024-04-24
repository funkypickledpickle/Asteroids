using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Asteroids.Configuration
{
    [CreateAssetMenu(menuName = "Configuration/GameWarmupConfiguration")]
    public class GamePreloadConfiguration : ScriptableObject
    {
        [FormerlySerializedAs("_entitiesWarmupQuantity")] [SerializeField] private int _preloadedEntitiesQuantity;

        public int PreloadedEntitiesQuantity => _preloadedEntitiesQuantity;
    }
}
