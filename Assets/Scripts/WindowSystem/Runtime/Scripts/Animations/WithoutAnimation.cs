using System.Threading.Tasks;
using UnityEngine;

namespace WindowSystem.Runtime.Scripts.Animations
{
    public class WithoutAnimation : WindowAnimation
    {
        private CanvasGroup _canvasGroup;

        public override void Initialize(WindowAnimator animator)
        {
            _canvasGroup = animator.GetComponent<CanvasGroup>();
        }

        public override Task Play()
        {
            _canvasGroup.alpha = 1f;
            return Task.CompletedTask;
        }
    }
}