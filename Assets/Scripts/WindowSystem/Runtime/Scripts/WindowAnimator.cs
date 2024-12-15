using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using WindowSystem.Runtime.Scripts.Animations;

namespace WindowSystem.Runtime.Scripts
{
    public class WindowAnimator : MonoBehaviour
    {
        private readonly Dictionary<WindowAnimationType, WindowAnimation> _animations = new()
        {
            { WindowAnimationType.NoAnimation, new WithoutAnimation() },
        
            { WindowAnimationType.SlideInHorizontal, new SlideInHorizontalAnimation() },
            { WindowAnimationType.SlideOutHorizontal, new SlideOutHorizontalAnimation() },
        
            { WindowAnimationType.SlideInVertical, new SlideInVerticalAnimation() },
            { WindowAnimationType.SlideOutVertical, new SlideOutVerticalAnimation() },
        
            { WindowAnimationType.FadeIn, new FadeInAnimation() },
            { WindowAnimationType.FadeOut, new FadeOutAnimation() }
        };
    
        private WindowAnimation _currentAnimation;

        public async Task Play(WindowAnimationType animationType)
        {
            _currentAnimation = _animations[animationType];
            _currentAnimation.Initialize(this);
            await _currentAnimation.Play();
        }
    }
}