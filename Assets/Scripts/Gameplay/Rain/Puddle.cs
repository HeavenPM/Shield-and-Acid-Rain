using DG.Tweening;
using Gameplay.Character;
using UnityEngine;

namespace Gameplay.Rain
{
    public class Puddle : MonoBehaviour
    {
        private const float DamageInterval = 1f;
        private const int DamagePerInterval = 1;

        private float _timer = 20f;
        private float _damageTimer;

        private CharacterMovement _movement;
        private CharacterStats _stats;
        private bool _containsCharacter;
        private Tween _scaleTween;

        private void OnEnable()
        {
            _scaleTween = transform
                .DOScale(transform.localScale, 1f)
                .From(Vector3.zero);
        }

        private void Update()
        {
            _timer -= Time.deltaTime;

            if (_containsCharacter)
            {
                _damageTimer -= Time.deltaTime;
                if (_damageTimer <= 0f)
                {
                    _stats?.ApplyDamage(DamagePerInterval, true);
                    _damageTimer = DamageInterval;
                }
            }

            if (_timer > 0) return;

            if (_containsCharacter) _movement?.OnGetOutOfPuddle();

            _scaleTween?.Kill();
            _scaleTween = transform
                .DOScale(Vector3.zero, 1f)
                .OnComplete(() => { if (this != null) Destroy(gameObject); });
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var characterFacade = GetCharacterFacade(other);
            if (characterFacade == null) return;

            _movement = characterFacade.Movement;
            _stats = characterFacade.Stats;

            _movement.OnSteppedInPuddle();
            _containsCharacter = true;
            _damageTimer = DamageInterval;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var characterFacade = GetCharacterFacade(other);
            if (characterFacade == null) return;

            _movement.OnGetOutOfPuddle();
            _containsCharacter = false;
        }

        private void OnDisable()
        {
            _scaleTween?.Kill();
            transform.DOKill();
        }

        private CharacterFacade GetCharacterFacade(Collider2D other)
            => other.TryGetComponent(out CharacterFacade characterFacade) ? characterFacade : null;
    }
}
