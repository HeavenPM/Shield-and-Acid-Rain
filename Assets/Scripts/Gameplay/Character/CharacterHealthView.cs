using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Character
{
    public class CharacterHealthView : MonoBehaviour
    {
        private readonly CompositeDisposable _disposables = new();
        
        [Inject] private CharacterFacade _character;

        private CharacterStats _stats;
        private Image _image;
        
        private void Awake()
        {
            _image = GetComponent<Image>();
        }
        
        private void Start()
        {
            _stats = _character.Stats;
            
            _stats.Health
                .StartWith(_stats.Health.Value)
                .Subscribe(OnHealthUpdated)
                .AddTo(_disposables);
        }

        private void OnDestroy() 
            => _disposables.Clear();
            
        private void OnHealthUpdated(int health)
            => _image.fillAmount = (float)health / _stats.MaxHealth;
    }
}