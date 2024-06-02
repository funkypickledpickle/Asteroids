using System;
using Asteroids.Gameplay.States;
using Asteroids.Installation;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.Services;
using Asteroids.Tools;
using Asteroids.UnsafeTools;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityGroup;
using TMPro;
using UnityEngine;
using Zenject;

namespace Asteroids.UI.Views
{
    [RequireComponent(typeof(Injector))]
    [DisallowMultipleComponent]
    public class GameView : MonoBehaviour, IInjectable
    {
        [SerializeField] private TMP_Text _positionLabel;
        [SerializeField] private TMP_Text _rotationLabel;
        [SerializeField] private TMP_Text _velocityLabel;
        [SerializeField] private TMP_Text _chargesCountLabel;
        [SerializeField] private TMP_Text _rechargeTimerLabel;

        [SerializeField] private TMP_Text _scoreLabel;

        [Inject] private readonly IStateContext _stateContext;
        [Inject] private readonly IInstanceSpawner _instanceSpawner;
        [Inject] private readonly IFrameInfoService _frameInfoService;

        private EntityGroup _playerGroup;
        private EntityGroup _scoreGroup;
        private EntityGroup _receivedScoreGroup;

        private Vector2MutableStringPresenter _positionText = new Vector2MutableStringPresenter(NumericFormats.G4);
        private SingleMutableStringPresenter _rotationText = new SingleMutableStringPresenter(NumericFormats.G4);
        private SingleMutableStringPresenter _velocityText = new SingleMutableStringPresenter(NumericFormats.G4);
        private IntegerMutableStringPresenter _chargesCountText = new IntegerMutableStringPresenter(NumericFormats.G4);
        private SingleMutableStringPresenter _rechargeTimerText = new SingleMutableStringPresenter(NumericFormats.G4);

        private IntegerMutableStringPresenter _scoreText = new IntegerMutableStringPresenter(new GenericFormat(10));

        private void Awake()
        {
            _playerGroup = _instanceSpawner.Instantiate<EntityGroupBuilder>()
                .RequireComponent<PlayerComponent>()
                .RequireComponent<PositionComponent>()
                .RequireComponent<RotationComponent>()
                .RequireComponent<VelocityComponent>()
                .RequireComponent<LaserGunComponent>()
                .RequireComponent<LaserAutoChargingComponent>()
                .Build();

            _scoreGroup = _instanceSpawner.Instantiate<EntityGroupBuilder>().RequireComponent<ScoreComponent>().Build();

            _receivedScoreGroup = _instanceSpawner.Instantiate<EntityGroupBuilder>().RequireComponent<ReceivedScoreComponent>().Build();
            _receivedScoreGroup.EntityRemoved += HandleScoreGained;
        }

        private void OnEnable()
        {
            UpdateScore();
        }

        private void OnDestroy()
        {
            _playerGroup.Dispose();
            _playerGroup = null;

            _scoreGroup.Dispose();
            _scoreGroup = null;

            _receivedScoreGroup.EntityRemoved -= HandleScoreGained;
            _receivedScoreGroup.Dispose();
            _receivedScoreGroup = null;
        }

        private void Update()
        {
            if (_playerGroup.Count == 0)
            {
                return;
            }

            ref Entity playerEntity = ref _playerGroup.GetFirst();
            ref PositionComponent positionComponent = ref playerEntity.GetComponent<PositionComponent>();
            ref RotationComponent rotationComponent = ref playerEntity.GetComponent<RotationComponent>();
            ref VelocityComponent velocityComponent = ref playerEntity.GetComponent<VelocityComponent>();
            ref LaserGunComponent laserGunComponent = ref playerEntity.GetComponent<LaserGunComponent>();

            _positionText.UpdateContent(positionComponent.Position);
            _positionLabel.SetText(_positionText.ToString());

            _rotationText.UpdateContent(rotationComponent.RotationDegrees);
            _rotationLabel.SetText(_rotationText.ToString());

            _velocityText.UpdateContent((velocityComponent.Velocity).magnitude);
            _velocityLabel.SetText(_velocityText.ToString());

            _chargesCountText.UpdateContent(laserGunComponent.ChargesCount);
            _chargesCountLabel.SetText(_chargesCountText.ToString());

            float laserReadyTime = laserGunComponent.LastFireTime + laserGunComponent.Configuration.FiringInterval;
            bool isLaserReady = laserReadyTime < _frameInfoService.StartTime;
            float timerValue = isLaserReady ? 0 : laserReadyTime - _frameInfoService.StartTime;
            _rechargeTimerText.UpdateContent(timerValue);
            _rechargeTimerLabel.SetText(_rechargeTimerText.ToString());
        }

        private void HandleScoreGained(ref Entity referenced)
        {
            UpdateScore();
        }

        private void UpdateScore()
        {
            ref ScoreComponent scoreComponent = ref _scoreGroup.GetFirst().GetComponent<ScoreComponent>();
            _scoreText.UpdateContent(scoreComponent.Score);
            _scoreLabel.SetText(_scoreText.ToString());
        }
    }
}
