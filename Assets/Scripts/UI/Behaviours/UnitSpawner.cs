using System.Collections.Generic;
using Asteroids.Installation;
using Asteroids.Configuration;
using Asteroids.GameplayECS.Components;
using Asteroids.Services.EntityView;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityContainer;
using Asteroids.ValueTypeECS.EntityGroup;
using UnityEngine;
using Zenject;

namespace Asteroids.UI.Behaviours
{
    [RequireComponent(typeof(Injector))]
    public class UnitSpawner : MonoBehaviour, IInjectable
    {
        [SerializeField] private GameViewConfiguration _gameViewConfiguration;

        [Inject] private readonly IEntityViewService _entityViewService;
        [Inject] private readonly IInstanceSpawner _instanceSpawner;
        [Inject] private readonly World _world;

        private EntityGroup _playerGroup;
        private EntityGroup _asteroidsGroup;
        private EntityGroup _ufoGroup;
        private EntityGroup _laserGroup;
        private EntityGroup _bulletGroup;

        private Dictionary<int, (ViewKey, GameObject)> _views = new Dictionary<int, (ViewKey, GameObject)>();

        public IDictionary<GameObject, int> Keys = new Dictionary<GameObject, int>();

        private void Awake()
        {
            _playerGroup = _instanceSpawner.Instantiate<EntityGroupBuilder>().RequireComponent<PlayerComponent>().RequireComponent<PositionComponent>().RequireComponent<RotationComponent>().Build();
            _asteroidsGroup = _instanceSpawner.Instantiate<EntityGroupBuilder>().RequireComponent<AsteroidComponent>().RequireComponent<PositionComponent>().RequireComponent<RotationComponent>().RequireComponent<ScaleComponent>().Build();
            _ufoGroup = _instanceSpawner.Instantiate<EntityGroupBuilder>().RequireComponent<UFOComponent>().RequireComponent<PositionComponent>().RequireComponent<RotationComponent>().Build();
            _bulletGroup = _instanceSpawner.Instantiate<EntityGroupBuilder>().RequireComponent<BulletComponent>().RequireComponent<PositionComponent>().RequireComponent<RotationComponent>().Build();
            _laserGroup = _instanceSpawner.Instantiate<EntityGroupBuilder>().RequireComponent<LaserComponent>().RequireComponent<ScaleComponent>().Build();

            _playerGroup.EntityAdded += HandlePlayerAdded;
            _playerGroup.EntityRemoved += HandleEntityRemoved;

            _ufoGroup.EntityAdded += HandleUfoAdded;
            _ufoGroup.EntityRemoved += HandleEntityRemoved;

            _asteroidsGroup.EntityAdded += HandleAsteroidAdded;
            _asteroidsGroup.EntityRemoved += HandleEntityRemoved;

            _bulletGroup.EntityAdded += HandleBulletAdded;
            _bulletGroup.EntityRemoved += HandleEntityRemoved;

            _laserGroup.EntityAdded += HandleLaserAdded;
            _laserGroup.EntityRemoved += HandleEntityRemoved;

            _world.WillClear += HandleWorldWillClear;
        }

        private void Start()
        {
            _entityViewService.Preload();
        }

        private void OnDestroy()
        {
            _playerGroup.EntityAdded -= HandlePlayerAdded;
            _playerGroup.EntityRemoved -= HandleEntityRemoved;

            _ufoGroup.EntityAdded -= HandleUfoAdded;
            _ufoGroup.EntityRemoved -= HandleEntityRemoved;

            _asteroidsGroup.EntityAdded -= HandleAsteroidAdded;
            _asteroidsGroup.EntityRemoved -= HandleEntityRemoved;

            _bulletGroup.EntityAdded -= HandleBulletAdded;
            _bulletGroup.EntityRemoved -= HandleEntityRemoved;

            _laserGroup.EntityAdded -= HandleLaserAdded;
            _laserGroup.EntityRemoved -= HandleEntityRemoved;

            _playerGroup.Dispose();
            _playerGroup = null;

            _ufoGroup.Dispose();
            _ufoGroup = null;

            _asteroidsGroup.Dispose();
            _asteroidsGroup = null;

            _bulletGroup.Dispose();
            _bulletGroup = null;

            _laserGroup.Dispose();
            _laserGroup = null;

            _world.WillClear -= HandleWorldWillClear;
        }

        private void HandleAsteroidAdded(ref Entity entity)
        {
            ref AsteroidComponent asteroidComponent = ref entity.GetComponent<AsteroidComponent>();
            AddEntity(ref entity, _gameViewConfiguration.AsteroidGroups[asteroidComponent.GroupConfigurationIndex]);
        }

        private void HandleUfoAdded(ref Entity referenced)
        {
            AddEntity(ref referenced, _gameViewConfiguration.UfoViewKey);
        }

        private void HandlePlayerAdded(ref Entity referenced)
        {
            AddEntity(ref referenced, _gameViewConfiguration.PlayerViewKey);
        }

        private void HandleBulletAdded(ref Entity referenced)
        {
            AddEntity(ref referenced, _gameViewConfiguration.BulletViewKey);
        }

        private void HandleLaserAdded(ref Entity referenced)
        {
            AddEntity(ref referenced, _gameViewConfiguration.LaserViewKey);
        }

        private void AddEntity(ref Entity entity, ViewKey viewKey)
        {
            GameObject gameObject = _entityViewService.Get(viewKey);
            int entityId = entity.Id;
            _views.Add(entityId, (viewKey, gameObject));
            Keys.Add(gameObject, entityId);

            if (entity.HasComponent<AttachedToEntityComponent>())
            {
                ref AttachedToEntityComponent attachedToEntityComponent = ref entity.GetComponent<AttachedToEntityComponent>();
                ref Entity parentEntity = ref _world.GetEntity(attachedToEntityComponent.EntityId);
                Transform viewTransform = gameObject.transform;
                viewTransform.SetParent(_views[parentEntity.Id].Item2.transform);
                viewTransform.localPosition = attachedToEntityComponent.PositionOffset;
                viewTransform.localRotation = Quaternion.identity;
            }

            UnitBehaviour unitId = gameObject.GetComponent<UnitBehaviour>();
            if (unitId == null)
            {
                Debug.LogError($"GameObject {gameObject} with ViewKey {viewKey} missing {typeof(UnitBehaviour)} component");
            }

            unitId.EntityId = entityId;
            unitId.NotifyWillBeVisible();

            gameObject.SetActive(true);
        }

        private void HandleEntityRemoved(ref Entity entity)
        {
            int entityId = entity.Id;
            (ViewKey, GameObject) view = _views[entityId];
            _entityViewService.Release(view.Item1, view.Item2);
            _views.Remove(entityId);
            Keys.Remove(view.Item2);
        }

        private void HandleWorldWillClear()
        {
            foreach (KeyValuePair<int, (ViewKey, GameObject)> kv in _views)
            {
                _entityViewService.Release(kv.Value.Item1, kv.Value.Item2);
            }

            _views.Clear();
            Keys.Clear();
        }
    }
}
