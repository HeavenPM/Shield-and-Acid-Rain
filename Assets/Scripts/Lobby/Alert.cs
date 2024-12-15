using DG.Tweening;
using UnityEngine;

namespace Lobby
{
    public class Alert : MonoBehaviour
    {
        private const float ShownPositionX = 750f;
        private const float HiddenPositionX = 2000f;
        private const float ShowAnimationDuration = 1f;
        private const float HideAnimationDuration = .5f;

        private Tween _tween;
        private bool _isShown;
    
        public void ToggleAlert()
        {
            if (_tween != null && _tween.active) return;
        
            if (_isShown) Hide();
            else Show();

            _isShown = !_isShown;
        }

        private void Show()
        {
            _tween = transform
                .DOLocalMoveX(ShownPositionX, ShowAnimationDuration)
                .SetEase(Ease.OutBounce);
        }

        private void Hide()
        {
            _tween = transform
                .DOLocalMoveX(HiddenPositionX, HideAnimationDuration)
                .SetEase(Ease.InQuad);
        }
    }
}
