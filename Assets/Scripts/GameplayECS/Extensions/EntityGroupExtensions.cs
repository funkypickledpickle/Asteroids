using Asteroids.ValueTypeECS.Components;
using Asteroids.ValueTypeECS.Delegates;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityContainer;
using Asteroids.ValueTypeECS.EntityGroup;

namespace Asteroids.GameplayECS.Extensions
{
    public delegate void ActionReference<TReferenced1, TReferenced2>(ref TReferenced1 referenced1,
        ref TReferenced2 referenced2) where TReferenced1 : struct where TReferenced2 : struct;

    public delegate void ActionReference<TReferenced1, TReferenced2, TReferenced3>(ref TReferenced1 referenced1,
        ref TReferenced2 referenced2, ref TReferenced3 referenced3) where TReferenced1 : struct
        where TReferenced2 : struct
        where TReferenced3 : struct;

    public delegate void ActionReference<TReferenced1, TReferenced2, TReferenced3, TReferenced4>(
        ref TReferenced1 referenced1, ref TReferenced2 referenced2, ref TReferenced3 referenced3,
        ref TReferenced4 referenced4) where TReferenced1 : struct
        where TReferenced2 : struct
        where TReferenced3 : struct
        where TReferenced4 : struct;

    public delegate void ActionReferenceValue<TReferenced1, TReferenced2, TValue>(ref TReferenced1 referenced1,
        ref TReferenced2 referenced2, TValue value) where TReferenced1 : struct where TReferenced2 : struct;

    public static class EntityGroupExtensions
    {
        public static void ForEach(this EntityGroup entityGroup, ActionReference<Entity> actionReference)
        {
            World world = entityGroup.World;
            foreach (int id in entityGroup) actionReference(ref world.GetEntity(id));
        }

        public static void ForEach<TValue>(this EntityGroup entityGroup,
            ActionReferenceValue<Entity, TValue> actionReference, TValue value)
        {
            World world = entityGroup.World;
            foreach (int id in entityGroup) actionReference(ref world.GetEntity(id), value);
        }

        public static void ForEachComponent<TComponent>(this EntityGroup entityGroup,
            ActionReference<Entity, TComponent> actionReference) where TComponent : struct, IECSComponent
        {
            World world = entityGroup.World;
            foreach (int id in entityGroup)
            {
                ref Entity entity = ref world.GetEntity(id);
                ref TComponent component = ref entity.GetComponent<TComponent>();
                actionReference(ref entity, ref component);
            }
        }

        public static void ForEachComponent<TComponent, TValue>(this EntityGroup entityGroup,
            ActionReferenceValue<Entity, TComponent, TValue> actionReference, TValue value)
            where TComponent : struct, IECSComponent
        {
            World world = entityGroup.World;
            foreach (int id in entityGroup)
            {
                ref Entity entity = ref world.GetEntity(id);
                ref TComponent component = ref entity.GetComponent<TComponent>();
                actionReference(ref entity, ref component, value);
            }
        }

        public static void ForEachComponents<TComponent1, TComponent2>(this EntityGroup entityGroup,
            ActionReference<Entity, TComponent1, TComponent2> actionReferenceRef2)
            where TComponent1 : struct, IECSComponent where TComponent2 : struct, IECSComponent
        {
            World world = entityGroup.World;
            foreach (int id in entityGroup)
            {
                ref Entity entity = ref world.GetEntity(id);
                ref TComponent1 component1 = ref entity.GetComponent<TComponent1>();
                ref TComponent2 component2 = ref entity.GetComponent<TComponent2>();
                actionReferenceRef2(ref entity, ref component1, ref component2);
            }
        }

        public static void ForEachComponents<TComponent1, TComponent2, TComponent3>(this EntityGroup entityGroup,
            ActionReference<Entity, TComponent1, TComponent2, TComponent3> actionReference)
            where TComponent1 : struct, IECSComponent
            where TComponent2 : struct, IECSComponent
            where TComponent3 : struct, IECSComponent
        {
            World world = entityGroup.World;
            foreach (int id in entityGroup)
            {
                ref Entity entity = ref world.GetEntity(id);
                ref TComponent1 component1 = ref entity.GetComponent<TComponent1>();
                ref TComponent2 component2 = ref entity.GetComponent<TComponent2>();
                ref TComponent3 component3 = ref entity.GetComponent<TComponent3>();
                actionReference(ref entity, ref component1, ref component2, ref component3);
            }
        }

        public static void ForEachOnlyComponents<TComponent1, TComponent2>(this EntityGroup entityGroup,
            ActionReference<TComponent1, TComponent2> actionReference) where TComponent1 : struct, IECSComponent
            where TComponent2 : struct, IECSComponent
        {
            World world = entityGroup.World;
            foreach (int id in entityGroup)
            {
                ref Entity entity = ref world.GetEntity(id);
                ref TComponent1 component1 = ref entity.GetComponent<TComponent1>();
                ref TComponent2 component2 = ref entity.GetComponent<TComponent2>();
                actionReference(ref component1, ref component2);
            }
        }

        public static void ForEachOnlyComponents<TComponent1, TComponent2, TValue>(this EntityGroup entityGroup,
            ActionReferenceValue<TComponent1, TComponent2, TValue> actionReference, TValue value)
            where TComponent1 : struct, IECSComponent where TComponent2 : struct, IECSComponent
        {
            World world = entityGroup.World;
            foreach (int id in entityGroup)
            {
                ref Entity entity = ref world.GetEntity(id);
                ref TComponent1 component1 = ref entity.GetComponent<TComponent1>();
                ref TComponent2 component2 = ref entity.GetComponent<TComponent2>();
                actionReference(ref component1, ref component2, value);
            }
        }

        public static ref Entity GetFirst(this EntityGroup entityGroup)
        {
            return ref entityGroup.World.GetEntity(entityGroup.IdsList[0]);
        }
    }
}
