using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace WindowSystem.Runtime.Scripts.Animations
{
    public class FadeInAnimation : WindowAnimation
    {
        private const float Duration = 1f;

        private CanvasGroup _canvasGroup;
        private float _screenHeight;

        public override void Initialize(WindowAnimator animator)
        {
            _canvasGroup = animator.GetComponent<CanvasGroup>();
        }

        public override async Task Play()
        {
            _canvasGroup.alpha = 0f;
            await _canvasGroup.DOFade(1f, Duration).SetEase(Ease.OutQuad).AsyncWaitForCompletion();
        }
    }
}