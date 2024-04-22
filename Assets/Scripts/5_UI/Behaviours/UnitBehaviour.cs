using System;
using UnityEngine;

namespace Asteroids.UI.Behaviours
{
    [DisallowMultipleComponent]
    public class UnitBehaviour : MonoBehaviour
    {
        public event Action WillBeVisible;

        public int EntityId { get; set; }

        public void NotifyWillBeVisible()
        {
            WillBeVisible?.Invoke();
        }
    }
}
