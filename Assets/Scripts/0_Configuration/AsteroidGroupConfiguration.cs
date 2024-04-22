using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Configuration
{
    [CreateAssetMenu(menuName = "Configuration/Asteroid Group Configuration")]
    public class AsteroidGroupConfiguration : ScriptableObject
    {
        [Serializable]
        public class StateInfo
        {
            [SerializeField] private AsteroidConfiguration _asteroidConfiguration;
            [SerializeField] private float _size;
            [SerializeField] private float _speedMultiplier;
            [SerializeField] private int _rewardedScore;
            [SerializeField] private int _quantity;

            public AsteroidConfiguration AsteroidConfiguration => _asteroidConfiguration;
            public float Size => _size;
            public float SpeedMultiplier => _speedMultiplier;

            public int RewardedScore => _rewardedScore;

            public int Quantity => _quantity;
        }

        [SerializeField] private List<StateInfo> _asteroidStates;
        [SerializeField, Range(0, 1)] private float _spawnProbabitity;

        public List<StateInfo> AsteroidStates => _asteroidStates;
        public float SpawnProbability => _spawnProbabitity;
    }
}
