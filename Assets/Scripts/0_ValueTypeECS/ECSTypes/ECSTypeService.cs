using System;
using System.Collections.Generic;

namespace Asteroids.ValueTypeECS.ECSTypes
{
    public interface IECSObject { }

    public static class ECSTypeService
    {
        private static uint _typesCount = 0;
        private static Dictionary<uint, Type> _types = new Dictionary<uint, Type>();

        private static class ECSObjectTypeInfo<T> where T : IECSObject
        {
            public static ECSTypeKey ECSTypeKey { get; }

            static ECSObjectTypeInfo()
            {
                ECSTypeKey = new ECSTypeKey(_typesCount++);
                _types.Add(ECSTypeKey.Key, typeof(T));
            }
        }

        public static ECSTypeKey GetType<T>() where T : IECSObject
        {
            return ECSObjectTypeInfo<T>.ECSTypeKey;
        }

        public static Type GetSystemType(ECSTypeKey typeKey)
        {
            return _types[typeKey.Key];
        }
    }
}
