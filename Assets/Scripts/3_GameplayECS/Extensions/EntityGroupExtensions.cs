using Asteroids.ValueTypeECS.Components;
using Asteroids.ValueTypeECS.Delegates;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;

namespace Asteroids.GameplayECS.Extensions
{
    public delegate void ActionReference<TReferenced1, TReferenced2>(ref TReferenced1 referenced1, ref TReferenced2 referenced2) where TReferenced1 : struct where TReferenced2 : struct;
    public delegate void ActionReference<TReferenced1, TReferenced2, TReferenced3>(ref TReferenced1 referenced1, ref TReferenced2 referenced2, ref TReferenced3 referenced3) where TReferenced1 : struct where TReferenced2 : struct where TReferenced3 : struct;
    public delegate void ActionReferenceValue<TReferenced1, TReferenced2, TValue>(ref TReferenced1 referenced1, ref TReferenced2 referenced2, TValue value) where TReferenced1 : struct where TReferenced2 : struct;

    public static class EntityGroupExtensions
    {
        public static void ForEach(this EntityGroup entityGroup, ActionReference<Entity> actionReference)
        {
            var world = entityGroup.World;
            foreach (var id in entityGroup)
            {
                actionReference(ref world.GetEntity(id));
            }
        }

        public static void ForEachComponent<TComponent>(this EntityGroup entityGroup, ActionReference<Entity, TComponent> actionReference) where TComponent : struct, IECSComponent
        {
            var world = entityGroup.World;
            foreach (var id in entityGroup)
            {
                ref var entity = ref world.GetEntity(id);
                ref var component = ref entity.GetComponent<TComponent>();
                actionReference(ref entity, ref component);
            }
        }

        public static void ForEachComponents<TComponent1, TComponent2>(this EntityGroup entityGroup, ActionReference<Entity, TComponent1, TComponent2> actionReferenceRef2) where TComponent1 : struct, IECSComponent where TComponent2 : struct, IECSComponent
        {
            var world = entityGroup.World;
            foreach (var id in entityGroup)
            {
                ref var entity = ref world.GetEntity(id);
                ref var component1 = ref entity.GetComponent<TComponent1>();
                ref var component2 = ref entity.GetComponent<TComponent2>();
                actionReferenceRef2(ref entity, ref component1, ref component2);
            }
        }

        public static void ForEachOnlyComponents<TComponent1, TComponent2>(this EntityGroup entityGroup, ActionReference<TComponent1, TComponent2> actionReference) where TComponent1 : struct, IECSComponent where TComponent2 : struct, IECSComponent
        {
            var world = entityGroup.World;
            foreach (var id in entityGroup)
            {
                ref var entity = ref world.GetEntity(id);
                ref var component1 = ref entity.GetComponent<TComponent1>();
                ref var component2 = ref entity.GetComponent<TComponent2>();
                actionReference(ref component1, ref component2);
            }
        }

        public static void ForEachOnlyComponents<TComponent1, TComponent2, TValue>(this EntityGroup entityGroup, ActionReferenceValue<TComponent1, TComponent2, TValue> actionReference, TValue value) where TComponent1 : struct, IECSComponent where TComponent2 : struct, IECSComponent
        {
            var world = entityGroup.World;
            foreach (var id in entityGroup)
            {
                ref var entity = ref world.GetEntity(id);
                ref var component1 = ref entity.GetComponent<TComponent1>();
                ref var component2 = ref entity.GetComponent<TComponent2>();
                actionReference(ref component1, ref component2, value);
            }
        }
    }
}
