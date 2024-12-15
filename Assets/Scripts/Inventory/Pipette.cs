using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory
{
    public class Pipette : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private string _colorId;

        private Vector2 _initialPosition;
        private ItemPreview _selectPreview;

        public event Action EndCrossing;
        public event Action<bool> Dragging;
        public event Action<InventoryCategory> Crossing;

        public void OnBeginDrag(PointerEventData eventData)
        {
            _selectPreview = null;
            Dragging?.Invoke(true);
            EndCrossing?.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
            => transform.position = eventData.position;

        public void OnEndDrag(PointerEventData eventData)
        {
            Dragging?.Invoke(false);
            
            if (_selectPreview == null)
            {
                transform
                    .DOLocalMove(_initialPosition, 1f)
                    .SetEase(Ease.OutBounce);
                return;
            }
            
            _selectPreview.OnPipetteDropped(_colorId);
            transform.localPosition = _initialPosition;
        }

        private void Awake()
            => _initialPosition = transform.localPosition;

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.TryGetComponent(out ItemPreview preview)) return;
            if (_selectPreview != null) return;

            _selectPreview = preview;
            Crossing?.Invoke(_selectPreview.Category);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.TryGetComponent(out ItemPreview _)) return;
            _selectPreview = null;
            EndCrossing?.Invoke();
        }
    }
}