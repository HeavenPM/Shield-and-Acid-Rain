using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gameplay.Controls
{
    public class JoystickInputHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private const float MaxOffset = 150f;

        private Vector2 _currentDirection;
        private Tween _tween;

        public Vector2 CurrentDirection => _currentDirection;

        public void OnBeginDrag(PointerEventData eventData)
            => _tween?.Kill();

        public void OnDrag(PointerEventData eventData)
        {
            var dragOffset = eventData.position - (Vector2)transform.parent.position;

            if (dragOffset.magnitude > MaxOffset)
                dragOffset = dragOffset.normalized * MaxOffset;

            transform.localPosition = dragOffset;

            _currentDirection = dragOffset.normalized;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _tween = transform
                .DOLocalMove(Vector3.zero, 0.5f)
                .SetEase(Ease.OutBounce);

            _currentDirection = Vector2.zero;
        }
    }
}