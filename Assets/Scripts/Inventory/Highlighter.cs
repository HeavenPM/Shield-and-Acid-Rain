using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Inventory
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Highlighter : MonoBehaviour
    {
        [SerializeField] private InventoryCategory _category;
        
        [Inject] private Pipette[] _pipettes;

        private Tween _tween;
        private CanvasGroup _canvasGroup;
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            _canvasGroup.alpha = 0f;
            
            foreach (var pipette in _pipettes)
            {
                pipette.Dragging += OnDragging;
                pipette.Crossing += OnCrossing;
                pipette.EndCrossing += OnEndCrossing;
            }
        }
        
        private void OnDisable()
        {
            foreach (var pipette in _pipettes)
            {
                pipette.Dragging += OnDragging;
                pipette.Crossing += OnCrossing;
                pipette.EndCrossing += OnEndCrossing;
            }
        }
        
        private void OnEndCrossing()
        {
            _image.color = Color.white;
        }

        private void OnCrossing(InventoryCategory category)
        {
            _image.color = category == _category ? Color.green : Color.white;
        }

        private void OnDragging(bool state)
        {
            _tween?.Kill();
            _tween = _canvasGroup.DOFade(state ? 1f : 0f, .5f);
        }
    }
}
