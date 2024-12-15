using Gameplay.Character;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.Rain
{
    public class RainService : IInitializable, ITickable
    {
        public readonly float CycleDuration = 30f;
        
        private readonly ReactiveProperty<bool> _isRainyWeather = new();
        private readonly ReactiveProperty<float> _timer = new();
        
        [Inject] private GameService _gameService;
        [Inject] private CharacterFacade _character;

        public IReadOnlyReactiveProperty<bool> IsRainyWeather  => _isRainyWeather;
        public IReadOnlyReactiveProperty<float> Timer  => _timer;

        public void Initialize()
        {
            _gameService.Preparing += OnGamePreparing;
        }

        public void Tick()
        {
            if (!_gameService.IsRunning.Value) return;

            _timer.Value -= Time.deltaTime;

            if (_timer.Value > 0f) return;
            _isRainyWeather.Value = !_isRainyWeather.Value;
            _timer.Value = CycleDuration;
        }
        
        private void OnGamePreparing()
        {
            _isRainyWeather.Value = false;
            _timer.Value = CycleDuration;
        }
    }
}
