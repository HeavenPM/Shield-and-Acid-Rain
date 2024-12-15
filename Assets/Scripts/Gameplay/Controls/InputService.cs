using Gameplay.Character;
using UnityEngine;
using Zenject;

namespace Gameplay.Controls
{
    public class InputService : MonoBehaviour
    {
        private const float SmoothTime = 0.1f;
        private const float InputThreshold = 0.1f;
        
        [Inject] private CharacterFacade _characterFacade;

        private CharacterMovement _characterMovement;
        private KeyboardInputHandler _keyboardInputHandler;
        private JoystickInputHandler _joystickInputHandler;
        private Vector2 _smoothedDirection;

        private void Start()
        {
            _characterMovement = _characterFacade.Movement;
            
            _keyboardInputHandler = gameObject.AddComponent<KeyboardInputHandler>();
            _joystickInputHandler = gameObject.AddComponent<JoystickInputHandler>();
        }

        private void Update()
        {
            var joystickDirection = _joystickInputHandler.CurrentDirection;
            var keyboardDirection = _keyboardInputHandler.CurrentDirection;
            var combinedDirection = joystickDirection + keyboardDirection;

            if (combinedDirection.magnitude < InputThreshold)
            {
                combinedDirection = Vector2.zero;
                _smoothedDirection = Vector2.zero;
            }
            else
            {
                _smoothedDirection = Vector2.Lerp(_smoothedDirection, combinedDirection.normalized, SmoothTime);

                if (_smoothedDirection.magnitude < 0.01f)
                    _smoothedDirection = Vector2.zero;
            }

            _characterMovement.Move(_smoothedDirection);
        }
    }
}