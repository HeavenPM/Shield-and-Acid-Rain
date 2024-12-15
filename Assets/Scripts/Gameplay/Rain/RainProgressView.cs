using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Rain
{
    public class RainProgressView : MonoBehaviour
    {
        private readonly CompositeDisposable _disposables = new();
        
        [Inject] private RainService _rainService;

        private Image _image;

        private void Awake()
            => _image = GetComponent<Image>();

        private void OnEnable() 
        {
            _rainService.Timer
                .StartWith(_rainService.Timer.Value)
                .Subscribe(OnTimerUpdated)
                .AddTo(_disposables);
        }
        
        private void OnDisable() 
            => _disposables.Clear();
            
        private void OnTimerUpdated(float value)
            => _image.fillAmount = value / _rainService.CycleDuration;
    }
}