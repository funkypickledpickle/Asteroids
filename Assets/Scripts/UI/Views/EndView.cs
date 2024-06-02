using Asteroids.Gameplay.States;
using Asteroids.Installation;
using Asteroids.GameplayECS.Components;
using Asteroids.GameplayECS.Extensions;
using Asteroids.Tools;
using Asteroids.UnsafeTools;
using Asteroids.ValueTypeECS.EntityGroup;
using TMPro;
using UnityEngine;
using Zenject;

namespace Asteroids.UI.Views
{
    [RequireComponent(typeof(Injector))]
    [DisallowMultipleComponent]
    public class EndView : MonoBehaviour, IInjectable
    {
        [SerializeField] private TMP_Text _scoreLabel;

        [Inject] private readonly IStateContext _stateContext;
        [Inject] private readonly IInstanceSpawner _instanceSpawner;

        private IntegerMutableStringPresenter _scoreText = new IntegerMutableStringPresenter(NumericFormats.G4);
        private EntityGroup _scoreGroup;

        private void Awake()
        {
            _scoreGroup = _instanceSpawner.Instantiate<EntityGroupBuilder>().RequireComponent<ScoreComponent>().Build();
        }

        private void OnDestroy()
        {
            _scoreGroup.Dispose();
            _scoreGroup = null;
        }

        private void OnEnable()
        {
            TrySetup();
        }

        private void TrySetup()
        {
            ref ScoreComponent scoreComponent = ref _scoreGroup.GetFirst().GetComponent<ScoreComponent>();
            _scoreText.UpdateContent(scoreComponent.Score);
            _scoreLabel.SetText(_scoreText.ToString());
        }
    }
}
