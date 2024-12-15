using System;
using AudioSlider;
using UniRx;

namespace Gameplay.Character
{
    public class CharacterStats
    {
        public readonly int MaxHealth = 50;

        private readonly GameService _gameService;
        private readonly IAudioPlayer _audioPlayer;
        private readonly ReactiveProperty<int> _health = new();

        public IReadOnlyReactiveProperty<int> Health => _health;
        public IReadOnlyReactiveProperty<bool> IsShieldActive;
        public IReadOnlyReactiveProperty<bool> IsBootsActive;

        public CharacterStats(
            GameService gameService,
            IAudioPlayer audioPlayer,
            IReadOnlyReactiveProperty<bool> isShieldActive, 
            IReadOnlyReactiveProperty<bool> isBootsActive)
        {
            _gameService = gameService;
            _audioPlayer = audioPlayer;
            IsShieldActive = isShieldActive;
            IsBootsActive = isBootsActive;
        }
        
        public void ApplyDamage(int damage, bool fromPuddle = false)
        {
            if (damage < 0) throw new ArgumentException();
            
            if (!_gameService.IsRunning.Value) return;
            if (IsShieldActive.Value && !fromPuddle) return;
            if (IsBootsActive.Value && fromPuddle) return;

            _health.Value -= damage;
            _audioPlayer.Play("audio_hit");

            if (_health.Value > 0) return;
            
            _health.Value = 0;
            _gameService.OnFailed();
        }
        
        public void Reset()
        {
            _health.Value = MaxHealth;
        }
    }
}