using System.Collections.Generic;
using Asteroids.ValueTypeECS.DataContainers;
using Asteroids.ValueTypeECS.ECSTypes;

namespace Asteroids.ValueTypeECS.Components
{
    public interface IComponentsContainer
    {
        ref TComponent CreateComponent<TComponent>(out int index) where TComponent : struct, IECSComponent;
        ref TComponent GetComponent<TComponent>(int index) where TComponent : struct, IECSComponent;
        void RemoveComponent<TComponent>(int index) where TComponent : struct, IECSComponent;
        void RemoveComponent(ECSTypeKey typeKey, int index);
    }

    public class ComponentsContainer : IComponentsContainer
    {
        private readonly int _arraySizeInSegmentedList;
        private readonly Dictionary<ECSTypeKey, ISegmentedList> _componentsCollection = new Dictionary<ECSTypeKey, ISegmentedList>(new ECSTypeKeyEqualityComparer());

        public ComponentsContainer(int segmentedListCapacity)
        {
            _arraySizeInSegmentedList = segmentedListCapacity;
        }

        public ref TComponent CreateComponent<TComponent>(out int index) where TComponent : struct, IECSComponent
        {
            var list = GetList<TComponent>();
            ref var container = ref list.Reserve();
            index = container.Index;
            if (!container.Initialized)
            {
                container.Initialized = true;
            }

            return ref container.Value;
        }

        public ref TComponent GetComponent<TComponent>(int index) where TComponent : struct, IECSComponent
        {
            var list = GetList<TComponent>();
            return ref list.GetReservedValue(index).Value;
        }

        public void RemoveComponent<TComponent>(int index) where TComponent : struct, IECSComponent
        {
            var list = GetList<TComponent>();
            list.Free(index);
        }

        public void RemoveComponent(ECSTypeKey typeKey, int index)
        {
            GetList(typeKey).Free(index);
        }

        private UnorderedSegmentedList<TComponent> GetList<TComponent>() where TComponent : struct, IECSComponent
        {
            var typeKey = ECSTypeService.GetType<TComponent>();
            if (_componentsCollection.TryGetValue(typeKey, out var value))
            {
                return (UnorderedSegmentedList<TComponent>)value;
            }

            var list = new UnorderedSegmentedList<TComponent>(_arraySizeInSegmentedList);
            _componentsCollection.Add(typeKey, list);
            return list;
        }

        private ISegmentedList GetList(ECSTypeKey typeKey)
        {
            return _componentsCollection[typeKey];
        }
    }
}
