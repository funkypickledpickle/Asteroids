using UnityEngine;

namespace Asteroids.UI.Behaviours
{
    public class PhysicsBehaviour : MonoBehaviour
    {
        private const float TargetFrameRate = 60;
        private const float PhysicsStep = 1 / TargetFrameRate;

        private void Update()
        {
            Physics2D.SyncTransforms();
            Physics2D.Simulate(PhysicsStep);
        }
    }
}
