using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Configuration.Project
{
    [CreateAssetMenu(menuName = "Configuration/ViewPathsContainer")]
    public class ViewPathsContainer : ScriptableObject
    {
        [SerializeField] private List<string> _paths;

        public IReadOnlyList<string> Paths => _paths;
    }
}
