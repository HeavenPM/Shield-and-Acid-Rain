using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Zenject;

namespace Gameplay.Rain
{
    public class RainStateView : MonoBehaviour
    {
        private readonly CompositeDisposable _disposables = new();
        
        [Inject] private RainService _rainService;
        [Inject] private ResourcesLoader _resourcesLoader;

        private Image _image;
        private Sprite _rainyWeatherSprite;
        private Sprite _clearWeatherSprite;

        private void Awake()
        {
            _image = GetComponent<Image>();

            _rainyWeatherSprite = _resourcesLoader.Get<Sprite>("sprite_rainy_weather");
            _clearWeatherSprite = _resourcesLoader.Get<Sprite>("sprite_clear_weather");
        }
        
        private void OnEnable() 
        {
            _rainService.IsRainyWeather
                .StartWith(_rainService.IsRainyWeather.Value)
                .Subscribe(OnIsRainyWeatherUpdated)
                .AddTo(_disposables);
        }
        
        private void OnDisable() 
            => _disposables.Clear();

        private void OnIsRainyWeatherUpdated(bool isRainy)
        {
            _image.sprite = isRainy ? _rainyWeatherSprite : _clearWeatherSprite;
            _image.color = isRainy ? Color.green : Color.white;
        }
    }
}