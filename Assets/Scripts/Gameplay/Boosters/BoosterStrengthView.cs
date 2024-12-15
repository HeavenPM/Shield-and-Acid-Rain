using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Boosters
{
    public class BoosterStrengthView : MonoBehaviour
    {
        private readonly CompositeDisposable _disposables = new();
        
        [SerializeField] private BoosterType _boosterType;

        [Inject] private BoostersService _boostersService;
        
        private Image _image;

        private void Awake()
            => _image = GetComponent<Image>();

        private void OnEnable() 
        {
            _boostersService.GetBoosterStrength(_boosterType)
                .StartWith(_boostersService.GetBoosterStrength(_boosterType).Value)
                .Subscribe(OnStrengthUpdated)
                .AddTo(_disposables);
        }
        
        private void OnDisable() 
            => _disposables.Clear();
            
        private void OnStrengthUpdated(float value)
            => _image.fillAmount = value;
    }
}