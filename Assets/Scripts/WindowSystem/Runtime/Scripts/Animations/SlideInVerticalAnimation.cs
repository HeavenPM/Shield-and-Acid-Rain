using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace WindowSystem.Runtime.Scripts.Animations
{
    public class SlideInVerticalAnimation: WindowAnimation
    {
        private const float Duration = .5f;

        private RectTransform _windowRectTransform;
        private CanvasGroup _canvasGroup;
        private float _screenHeight;

        public override void Initialize(WindowAnimator animator)
        {
            _windowRectTransform = animator.GetComponent<RectTransform>();
            _canvasGroup = animator.GetComponent<CanvasGroup>();
            _screenHeight = _windowRectTransform.rect.height;
        }

        public override async Task Play()
        {
            _canvasGroup.alpha = 1f;
            var localPosition = _windowRectTransform.localPosition;
            localPosition = new Vector2(localPosition.x, localPosition.y - _screenHeight);
            _windowRectTransform.localPosition = localPosition;

            await _windowRectTransform.DOLocalMoveY(0f, Duration).SetEase(Ease.OutBounce).AsyncWaitForCompletion();
        }
    }
}