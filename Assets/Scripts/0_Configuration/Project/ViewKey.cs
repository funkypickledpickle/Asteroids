using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Configuration.Project
{
    [Serializable]
    public struct ViewKey
    {
        [SerializeField] private uint _key;
        public uint Key => _key;

        public ViewKey(uint key)
        {
            _key = key;
        }
    }
}
