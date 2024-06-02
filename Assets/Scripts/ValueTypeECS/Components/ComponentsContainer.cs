using System.Collections.Generic;
using System.Diagnostics;
using Asteroids.Configuration;
using Asteroids.ValueTypeECS.DataContainers;
using Asteroids.ValueTypeECS.ECSTypes;
using Debug = UnityEngine.Debug;

namespace Asteroids.ValueTypeECS.Components
{
    public interface IComponentsContainer
    {
        ref TComponent CreateComponent<TComponent>(out int id) where TComponent : struct, IECSComponent;
        ref TComponent GetComponent<TComponent>(int index) where TComponent : struct, IECSComponent;
        void RemoveComponent<TComponent>(int id) where TComponent : struct, IECSComponent;
        void RemoveComponent(ECSTypeKey typeKey, int id);
        void Reset();
    }

    public class ComponentsContainer : IComponentsContainer
    {
        private readonly int _arraySizeInSegmentedList;
        private readonly Dictionary<ECSTypeKey, ISegmentedList> _componentsCollection = new Dictionary<ECSTypeKey, ISegmentedList>(new ECSTypeKeyEqualityComparer());

        public ComponentsContainer(ContainersConfiguration configuration)
        {
            _arraySizeInSegmentedList = configuration.ArraySizeForComponentsSegmentedList;
        }

        public ref TComponent CreateComponent<TComponent>(out int id) where TComponent : struct, IECSComponent
        {
            UnorderedSegmentedList<TComponent> list = GetList<TComponent>();
            ref ValueContainer<TComponent> valueContainer = ref list.Reserve();
            id = valueContainer.Index;
            Log($"<color=\"green\">CreateComponent {typeof(TComponent)} with id {id}</color>");
            return ref valueContainer.Value;
        }

        public ref TComponent GetComponent<TComponent>(int index) where TComponent : struct, IECSComponent
        {
            UnorderedSegmentedList<TComponent> list = GetList<TComponent>();
            return ref list.GetReservedValue(index).Value;
        }

        public void RemoveComponent<TComponent>(int id) where TComponent : struct, IECSComponent
        {
            UnorderedSegmentedList<TComponent> list = GetList<TComponent>();
            list.Free(id);
            Log($"<color=\"red\">RemoveComponent {typeof(TComponent)} with id {id}</color>");
        }

        public void RemoveComponent(ECSTypeKey typeKey, int id)
        {
            GetList(typeKey).Free(id);
        }

        public void Reset()
        {
            foreach (KeyValuePair<ECSTypeKey, ISegmentedList> segmentedList in _componentsCollection)
            {
                segmentedList.Value.Clear();
            }
        }

        private UnorderedSegmentedList<TComponent> GetList<TComponent>() where TComponent : struct, IECSComponent
        {
            ECSTypeKey typeKey = ECSTypeService.GetType<TComponent>();
            if (_componentsCollection.TryGetValue(typeKey, out ISegmentedList value))
            {
                return (UnorderedSegmentedList<TComponent>)value;
            }

            UnorderedSegmentedList<TComponent> list = new UnorderedSegmentedList<TComponent>(_arraySizeInSegmentedList);
            _componentsCollection.Add(typeKey, list);
            return list;
        }

        private ISegmentedList GetList(ECSTypeKey typeKey)
        {
            return _componentsCollection[typeKey];
        }

        [Conditional("LOG_ENTITY_COMPONENTS")]
        private void Log(string text)
        {
            Debug.Log(text);
        }
    }
}
