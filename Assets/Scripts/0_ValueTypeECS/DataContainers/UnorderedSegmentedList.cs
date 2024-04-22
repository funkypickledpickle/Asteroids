using System;
using System.Collections.Generic;
using System.Linq;

namespace Asteroids.ValueTypeECS.DataContainers
{
    public struct ValueContainer<TValue> where TValue : struct
    {
        public readonly int Index;
        public bool IsInitialized { get; private set; }
        public TValue Value;

        public ValueContainer(int index)
        {
            Index = index;
            Value = default;
            IsInitialized = false;
        }

        public void Initialize(TValue value)
        {
            IsInitialized = true;
            Value = value;
        }
    }

    public interface ISegmentedList
    {
        public int? MaxReservedIndex { get; }
        public bool IsEmpty { get; }
        public bool IsIndexReserved(int index);
        public void Free(int index);
        public void Clear();
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

        private int? _maxReservedIndex;
        int? ISegmentedList.MaxReservedIndex => _maxReservedIndex;

        public bool IsEmpty => !_maxReservedIndex.HasValue;

        public UnorderedSegmentedList(int arraySize)
        {
            _arraySize = arraySize;
            CreateArray();
        }

        bool ISegmentedList.IsIndexReserved(int index)
        {
            if (IsEmpty || index < 0 || index > _maxReservedIndex)
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
            var targetIndex = isUnusedItemAvailable ? _freeIndices.First() : (_maxReservedIndex ?? -1) + 1;
            var indices = CalculateIndices(targetIndex);
            var array = TryGetOrCreateArray(indices.arrayIndex);
            if (!isUnusedItemAvailable)
            {
                _maxReservedIndex = targetIndex;
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
            if (index == _maxReservedIndex)
            {
                _maxReservedIndex--;
                while (_maxReservedIndex >= 0 && GetValueContainer(_maxReservedIndex.Value).Reserved == false)
                {
                    _freeIndices.Remove(_maxReservedIndex.Value);
                    _maxReservedIndex--;
                }

                if (_maxReservedIndex == -1)
                {
                    _maxReservedIndex = null;
                }
            }
            else
            {
                _freeIndices.Add(index);
                GetValueContainer(index).Reserved = false;
            }
        }

        public void Clear()
        {
            _maxReservedIndex = null;
            _freeIndices.Clear();
        }

        private ref ItemContainer<ValueContainer<TValue>> GetValueContainer(int index)
        {
            var indices = CalculateIndices(index);
            var array = GetArray(indices.arrayIndex);
            return ref array[indices.localItemIndex];
        }

        private void AssertIndexReserved(int index)
        {
            if (IsEmpty || index < 0 || index > _maxReservedIndex)
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
