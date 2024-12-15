using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace WindowSystem.Runtime.Scripts.Animations
{
    public class SlideOutVerticalAnimation : WindowAnimation
    {
        private const float Duration = .5f;

        private RectTransform _windowRectTransform;
        private CanvasGroup _canvasGroup;
        private float _windowHeight;

        public override void Initialize(WindowAnimator animator)
        {
            _windowRectTransform = animator.GetComponent<RectTransform>();
            _canvasGroup = animator.GetComponent<CanvasGroup>();
            _windowHeight = _windowRectTransform.rect.height;
        }

        public override async Task Play()
        {
            _canvasGroup.alpha = 1f;
            await _windowRectTransform.DOLocalMoveY(-_windowHeight, Duration).SetEase(Ease.InBounce).AsyncWaitForCompletion();
        }
    }
}