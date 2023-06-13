using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Configuration.Project
{
    [CreateAssetMenu(menuName = "Configuration/Configuration Container")]
    public class UnityConfigurationContainer : ScriptableObject
    {
        [SerializeField] private List<ScriptableObject> _configurations;

        public IReadOnlyList<ScriptableObject> Configurations => _configurations;
    }
}
