using System;
using AudioSlider;
using UniRx;
using WindowSystem.Runtime.Scripts;
using Zenject;

namespace Gameplay
{
    public class GameService
    {
        private readonly ReactiveProperty<bool> _isRunning = new();

        [Inject] private WindowsManager _windowsManager;
        [Inject] private IAudioPlayer _audioPlayer;
        
        public event Action Preparing;
        
        public IReadOnlyReactiveProperty<bool> IsRunning => _isRunning;

        public void OnFailed()
        {
            if (!_isRunning.Value) return;
            
            _isRunning.Value = false;
            _audioPlayer.Play("audio_fail");
            _windowsManager.ShowOver("popup_fail", animationType: WindowAnimationType.SlideInVertical);
        }
        
        public Action Prepare()
        {
            _isRunning.Value = false;
            Preparing?.Invoke();

            return OnStarted;
        }

        private void OnStarted()
        {
            _isRunning.Value = true;
        }
    }
}