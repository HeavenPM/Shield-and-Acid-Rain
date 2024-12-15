using UnityEngine;

namespace Gameplay.Controls
{
    public class KeyboardInputHandler : MonoBehaviour
    {
        private Vector2 _currentDirection;

        public Vector2 CurrentDirection => _currentDirection;

        private void Update()
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");

            _currentDirection = new Vector2(horizontal, vertical).normalized;
        }
    }
}