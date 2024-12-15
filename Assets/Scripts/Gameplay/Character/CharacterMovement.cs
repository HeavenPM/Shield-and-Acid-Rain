using UniRx;
using UnityEngine;

namespace Gameplay.Character
{
    public class CharacterMovement
    {
        private const float BaseSpeed = 5;
        private const float SlowDownPercentage = .25f;
        
        private readonly Transform _transform;
        private readonly ReactiveProperty<CharacterState> _state;
        
        private int _puddlesCount;
        
        public IReadOnlyReactiveProperty<bool> IsBootsActive;
        
        public CharacterMovement(
            Transform transform, 
            ReactiveProperty<CharacterState> state, 
            IReadOnlyReactiveProperty<bool> isBootsActive)
        {
            _transform = transform;
            _state = state;
            IsBootsActive = isBootsActive;
        }

        public void OnSteppedInPuddle() => _puddlesCount++;
        public void OnGetOutOfPuddle() => _puddlesCount--;
        
        public void Move(Vector2 direction)
        {
            if (direction == Vector2.zero)
            {
                _state.Value = CharacterState.Idle;
                return;
            }

            _state.Value = CharacterState.Walking;
            
            var speed = BaseSpeed;
            if (!IsBootsActive.Value && _puddlesCount > 0) speed *= (1f - SlowDownPercentage);
            
            var moveDirection = new Vector3(direction.x, direction.y);
            _transform.position += moveDirection * speed * Time.deltaTime;
            _transform.localScale = new Vector3(direction.x < 0f ? -1f : 1f, 1f);
        }

        public void Reset()
        {
            _transform.position = Vector3.zero;
            _puddlesCount = 0;
            _state.Value = CharacterState.Idle;
        }
    }
}