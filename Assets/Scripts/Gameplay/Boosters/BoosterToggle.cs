using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Zenject;

namespace Gameplay.Boosters
{
    public class BoosterToggle : UIController
    {
        private readonly CompositeDisposable _disposables = new();
    
        [SerializeField] private BoosterType _boosterType;
    
        [Inject] private BoostersService _boostersService;

        public override void HandleListener()
            => _boostersService.ToggleBooster(_boosterType);

        protected override void Awake()
        {
            base.Awake();

            Button.transition = Selectable.Transition.ColorTint;
        }

        protected override void OnEnable() 
        {
            base.OnEnable();
            _boostersService.GetBoosterStrength(_boosterType)
                .StartWith(_boostersService.GetBoosterStrength(_boosterType).Value)
                .Subscribe(OnStrengthChanged)
                .AddTo(_disposables);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _disposables.Clear();
        }
        
        private void OnStrengthChanged(float value)
            => Button.interactable = value != 0f;
    }
}
