using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.Boosters
{
    public class BoosterView : MonoBehaviour
    {
        private readonly CompositeDisposable _disposables = new();
        
        [SerializeField] private BoosterType _boosterType;

        [Inject] private BoostersService _boostersService;

        private SpriteRenderer _spriteRenderer;
        private Collider2D _collider2D;

        public BoosterType BoosterType => _boosterType;
        public Collider2D Collider2D => _collider2D;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider2D = GetComponent<Collider2D>();
        }

        private void OnEnable() 
        {
            _boostersService.GetBoosterActiveState(_boosterType)
                .StartWith(_boostersService.GetBoosterActiveState(_boosterType).Value)
                .Subscribe(OnBoosterStateUpdated)
                .AddTo(_disposables);
        }
        
        private void OnDisable() 
            => _disposables.Clear();
            
        private void OnBoosterStateUpdated(bool state)
        {
            _spriteRenderer.color = state ? Color.white : new Color(0f, 0f, 0f, .003f);
            _collider2D.enabled = state;
        }
    }
}