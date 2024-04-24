using System;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using Asteroids.ValueTypeECS.System;

namespace Asteroids.GameplayECS.Systems.Score
{
    public class ScoreCollectingSystem : ISystem, IDisposable
    {
        private EntityGroup _entityGroup;
        private EntityGroup _scoreGroup;

        public ScoreCollectingSystem(IInstanceSpawner instanceSpawner)
        {
            _entityGroup = instanceSpawner.Instantiate<EntityGroupBuilder>().RequireComponent<ReceivedScoreComponent>().Build();
            _entityGroup.EntityAdded += EntityAddedHandler;

            _scoreGroup = instanceSpawner.Instantiate<EntityGroupBuilder>()
             .RequireComponent<ScoreComponent>()
             .Build();
        }

        public void Dispose()
        {
            _entityGroup.EntityAdded -= EntityAddedHandler;
            _entityGroup.Dispose();
            _entityGroup = null;

            _scoreGroup.Dispose();
            _scoreGroup = null;
        }

        private void EntityAddedHandler(ref Entity scoreEntity)
        {
            ref var scoreGainedComponent = ref scoreEntity.GetComponent<ReceivedScoreComponent>();
            if (_scoreGroup.Count != 0)
            {
                _scoreGroup.GetFirst().GetComponent<ScoreComponent>().Score += scoreGainedComponent.Score;
            }

            scoreEntity.CreateComponent<DestroyedComponent>();
        }
    }
}
