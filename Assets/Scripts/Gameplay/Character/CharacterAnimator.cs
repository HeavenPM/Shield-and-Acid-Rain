using System;
using UniRx;
using UnityEngine;

namespace Gameplay.Character
{
    public class CharacterAnimator
    {
        private readonly Animator _animator;
        
        private CharacterState _state;

        public CharacterAnimator(Animator animator, IObservable<CharacterState> state)
        {
            _animator = animator;
            
            state
                .StartWith(CharacterState.Idle)
                .Subscribe(OnUpdated);
        }

        public void Reset()
        {
        }

        private void OnUpdated(CharacterState state)
        {
            if (state == _state) return;

            _state = state;
            _animator.SetTrigger($"{_state}");
        }
    }
}