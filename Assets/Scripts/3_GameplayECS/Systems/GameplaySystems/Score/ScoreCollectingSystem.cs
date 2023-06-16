using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;

namespace Asteroids.GameplayECS.Systems.Score
{
    public class ScoreCollectingSystem : AbstractSystem
    {
        private EntityGroup _scoreGroup;

        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>().RequireComponent<ReceivedScoreComponent>().Build();
        }

        protected override void InitializeInternal()
        {
            EntityGroup.SubscribeToEntityAddedEvent(EntityAddedHandler);
            _scoreGroup = InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<ScoreComponent>()
               .Build();
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
