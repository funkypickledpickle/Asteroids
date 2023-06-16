using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.ValueTypeECS.EntityGroup;

namespace Asteroids.GameplayECS.Systems.World
{
    public class WorldMirroringSystem : AbstractExecutableSystem
    {
        private EntityGroup _entityGroup;

        protected override EntityGroup CreateContainer()
        {
            return InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<PositionComponent>()
               .RequireComponent<VelocityComponent>()
               .Build();
        }

        protected override void InitializeInternal()
        {
            _entityGroup = InstanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<WorldBoundsComponent>()
               .Build();
        }

        public override void Execute()
        {
            if (_entityGroup.Count != 0)
            {
                ref var bounds = ref _entityGroup.GetFirst().GetComponent<WorldBoundsComponent>().Bounds;

                foreach (var entityId in EntityGroup)
                {
                    ref var entity = ref World.GetEntity(entityId);
                    ref var positionComponent = ref entity.GetComponent<PositionComponent>();
                    ref var position = ref positionComponent.Position;
                    if (position.x < bounds.xMin)
                    {
                        position.x += bounds.size.x;
                    }
                    else if (position.x > bounds.xMax)
                    {
                        position.x -= bounds.size.x;
                    }

                    if (position.y < bounds.yMin)
                    {
                        position.y += bounds.size.y;
                    }
                    else if (position.y > bounds.yMax)
                    {
                        position.y -= bounds.size.y;
                    }
                }
            }
        }
    }
}
