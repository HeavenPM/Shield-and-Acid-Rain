using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace WindowSystem.Runtime.Scripts.Animations
{
    public class SlideInHorizontalAnimation : WindowAnimation
    {
        private const float Duration = 1f;

        private RectTransform _windowRectTransform;
        private CanvasGroup _canvasGroup;
        private float _screenWidth;

        public override void Initialize(WindowAnimator animator)
        {
            _canvasGroup = animator.GetComponent<CanvasGroup>();
            _windowRectTransform = animator.GetComponent<RectTransform>();
            _screenWidth = _windowRectTransform.rect.width;
        }

        public override async Task Play()
        {
            _canvasGroup.alpha = 1f;
            var localPosition = _windowRectTransform.localPosition;
            localPosition = new Vector2(localPosition.x + _screenWidth, localPosition.y);
            _windowRectTransform.localPosition = localPosition;

            await _windowRectTransform.DOLocalMoveX(0f, Duration).SetEase(Ease.OutBounce).AsyncWaitForCompletion();
        }
    }
}