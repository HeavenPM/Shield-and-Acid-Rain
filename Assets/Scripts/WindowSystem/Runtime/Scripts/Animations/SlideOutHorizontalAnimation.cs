using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace WindowSystem.Runtime.Scripts.Animations
{
    public class SlideOutHorizontalAnimation : WindowAnimation
    {
        private const float Duration = 1f;

        private RectTransform _windowRectTransform;
        private CanvasGroup _canvasGroup;
        private float _screenWidth;

        public override void Initialize(WindowAnimator animator)
        {
            _windowRectTransform = animator.GetComponent<RectTransform>();
            _canvasGroup = animator.GetComponent<CanvasGroup>();
            _screenWidth = _windowRectTransform.rect.width;
        }

        public override async Task Play()
        {
            _canvasGroup.alpha = 1f;
            await _windowRectTransform.DOLocalMoveX(_screenWidth, Duration).SetEase(Ease.InBounce).AsyncWaitForCompletion();
        }
    }
}