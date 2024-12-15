using AudioSlider;
using Gameplay.Boosters;
using Gameplay.Camera;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.Character
{
    public class CharacterFacade : MonoBehaviour
    {
        private readonly ReactiveProperty<CharacterState> _state = new();

        [Inject] private GameService _gameService;
        [Inject] private CameraTracker _cameraTracker;
        [Inject] private BoostersService _boostersService;
        [Inject] private IAudioPlayer _audioPlayer;

        private CharacterMovement _movement;
        private CharacterAnimator _animator;
        private CharacterStats _stats;

        public CharacterMovement Movement => _movement;
        public CharacterStats Stats => _stats;
        public IReadOnlyReactiveProperty<CharacterState> State => _state;

        private void Awake()
        {
            var isShieldActive = _boostersService.GetBoosterActiveState(BoosterType.Shield);
            var isBootsActive = _boostersService.GetBoosterActiveState(BoosterType.Boots);

            _stats = new CharacterStats(_gameService, _audioPlayer, isShieldActive, isBootsActive);
            _movement = new CharacterMovement(transform, _state, isBootsActive);
            _animator = new CharacterAnimator(GetComponent<Animator>(), State);

            _cameraTracker.SetTarget(transform);
        }

        private void OnEnable()
        {
            OnGamePreparing();
            _gameService.Preparing += OnGamePreparing;
        }

        private void OnDisable()
        {
            _gameService.Preparing -= OnGamePreparing;
        }

        private void OnGamePreparing()
        {
            _movement.Reset();
            _animator.Reset();
            _stats.Reset();
        }
    }
}
