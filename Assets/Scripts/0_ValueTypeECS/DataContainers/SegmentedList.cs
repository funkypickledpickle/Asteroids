using System;
using System.Collections.Generic;
using System.Linq;

namespace Asteroids.ValueTypeECS.DataContainers
{
    public struct ValueContainer<TValue> where TValue : struct
    {
        public readonly int Index;
        public bool Initialized;
        public TValue Value;

        public ValueContainer(int index)
        {
            Index = index;
            Value = default;
            Initialized = false;
        }
    }

    public interface ISegmentedList
    {
        int? MaxReservedIndex { get; }
        bool IsEmpty { get; }
        bool IsIndexReserved(int index);
        void Free(int index);
    }

    public sealed class UnorderedSegmentedList<TValue> : ISegmentedList where TValue : struct
    {
        private struct ItemContainer<T> where T : struct
        {
            public bool Initialized;
            public bool Reserved;
            public T Value;
        }

        private readonly int _arraySize;
        private List<Array> _arrayList = new List<Array>();
        private List<int> _freeIndices = new List<int>();

        public int? MaxReservedIndex { get; private set; }

        public bool IsEmpty => !MaxReservedIndex.HasValue;

        public UnorderedSegmentedList(int arraySize)
        {
            _arraySize = arraySize;
            CreateArray();
        }

        public bool IsIndexReserved(int index)
        {
            if (IsEmpty || index < 0 || index > MaxReservedIndex)
            {
                return false;
            }

            var indices = CalculateIndices(index);
            var array = GetArray(indices.arrayIndex);
            return array[indices.localItemIndex].Reserved;
        }

        public ref ValueContainer<TValue> GetReservedValue(int index)
        {
            AssertIndexReserved(index);
            return ref GetValueContainer(index).Value;
        }

        public ref ValueContainer<TValue> Reserve()
        {
            var isUnusedItemAvailable = _freeIndices.Count != 0;
            var targetIndex = isUnusedItemAvailable ? _freeIndices.First() : (MaxReservedIndex ?? -1) + 1;
            var indices = CalculateIndices(targetIndex);
            var array = TryGetOrCreateArray(indices.arrayIndex);
            if (!isUnusedItemAvailable)
            {
                MaxReservedIndex = targetIndex;
            }
            else
            {
                _freeIndices.Remove(targetIndex);
            }

            ref var itemContainer = ref array[indices.localItemIndex];
            if (!itemContainer.Initialized)
            {
                itemContainer.Value = new ValueContainer<TValue>(targetIndex);
                itemContainer.Initialized = true;
            }

            itemContainer.Reserved = true;

            return ref itemContainer.Value;
        }

        public void Free(int index)
        {
            AssertIndexReserved(index);
            if (index == MaxReservedIndex)
            {
                MaxReservedIndex--;
                while (MaxReservedIndex >= 0 && GetValueContainer(MaxReservedIndex.Value).Reserved == false)
                {
                    _freeIndices.Remove(MaxReservedIndex.Value);
                    MaxReservedIndex--;
                }

                if (MaxReservedIndex == -1)
                {
                    MaxReservedIndex = null;
                }
            }
            else
            {
                _freeIndices.Add(index);
                GetValueContainer(index).Reserved = false;
            }
        }

        private ref ItemContainer<ValueContainer<TValue>> GetValueContainer(int index)
        {
            var indices = CalculateIndices(index);
            var array = GetArray(indices.arrayIndex);
            return ref array[indices.localItemIndex];
        }

        private void AssertIndexReserved(int index)
        {
            if (IsEmpty || index < 0 || index > MaxReservedIndex)
            {
                throw new IndexOutOfRangeException();
            }

            var indices = CalculateIndices(index);
            var array = GetArray(indices.arrayIndex);
            if (!array[indices.localItemIndex].Initialized || !array[indices.localItemIndex].Reserved)
            {
                throw new ArgumentException("The index is not used and it may has incorrect data");
            }
        }

        private (int arrayIndex, int localItemIndex) CalculateIndices(int index)
        {
            return (index / _arraySize, index % _arraySize);
        }

        private ItemContainer<ValueContainer<TValue>>[] GetArray(int arrayIndex)
        {
            return (ItemContainer<ValueContainer<TValue>>[])_arrayList[arrayIndex];
        }

        private ItemContainer<ValueContainer<TValue>>[] CreateArray()
        {
            var array = new ItemContainer<ValueContainer<TValue>>[_arraySize];
            _arrayList.Add(array);
            return array;
        }

        private ItemContainer<ValueContainer<TValue>>[] TryGetOrCreateArray(int arrayIndex)
        {
            if (arrayIndex >= _arrayList.Count)
            {
                return CreateArray();
            }

            return GetArray(arrayIndex);
        }

        public SegmentedListEnumerator GetEnumerator()
        {
            return new SegmentedListEnumerator(this);
        }
    }

    public struct SegmentedListEnumerator
    {
        private int _index;
        private ISegmentedList _list;

        public SegmentedListEnumerator(ISegmentedList list)
        {
            _list = list;
            _index = -1;
        }

        public bool MoveNext()
        {
            do
            {
                _index++;

                if (_list.IsEmpty || _index > _list.MaxReservedIndex)
                {
                    return false;
                }
            } while (!_list.IsIndexReserved(_index));

            return true;
        }

        public void Reset()
        {
            _index = -1;
        }

        public int Current => _index;
    }
}
