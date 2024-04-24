using Asteroids.ValueTypeECS.Components;
using UnityEngine;

namespace Asteroids.GameplayECS.Components
{
    public struct MassComponent : IECSComponent
    {
        public float Mass;
    }

    public struct UpdatableForceComponent : IECSComponent
    {
        public bool IsApplied => Force != Vector2.zero;
        public Vector2 Force;
    }

    public struct VelocityComponent : IECSComponent
    {
        public Vector2 Velocity;
    }

    public struct VelocityLimiterComponent : IECSComponent
    {
        public float MaxSpeed;
    }

    public struct VelocityDumpComponent : IECSComponent
    {
        public float StartFactor;
        public float TotalFactor;
    }

    public struct UpdatableAngularForceComponent : IECSComponent
    {
        public bool IsApplied => AngularForce != 0;
        public float AngularForce;
    }

    public struct AngularVelocityComponent : IECSComponent
    {
        public float AngularSpeed;
    }

    public struct AngularVelocityLimiterComponent : IECSComponent
    {
        public float MaxSpeed;
    }

    public struct AngularVelocityDumpComponent : IECSComponent
    {
        public float StartFactor;
        public float TotalFactor;
    }

    public struct CollisionComponent : IECSComponent
    {
        public int EntityId;
    }
}
