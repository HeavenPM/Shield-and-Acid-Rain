using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Extensions
{
    public static class GraphicExtensions
    {
        private const float TweeningDelay = 0.1f;
    
        private static readonly Dictionary<Graphic, bool> IsAnimatingDictionary = new();

        public static float EnlargeAndHighlight(this Graphic graphic, Action callback = null, float animationDuration = 0.5f, float scaleMultiplier = 1.5f)
        {
            if (IsAnimatingDictionary.ContainsKey(graphic) && IsAnimatingDictionary[graphic])
                return animationDuration + TweeningDelay;

            IsAnimatingDictionary[graphic] = true;

            var originalScale = graphic.transform.localScale;
            var originalColor = graphic.color;

            graphic.transform.DOScale(originalScale * scaleMultiplier, animationDuration / 2)
                .OnComplete(() =>
                {
                    graphic.transform.DOScale(originalScale, animationDuration / 2)
                        .OnComplete(() => IsAnimatingDictionary[graphic] = false);
                });

            graphic.DOColor(new Color(originalColor.r, originalColor.g, originalColor.b, 0.7f), animationDuration / 2)
                .OnComplete(() =>
                {
                    graphic.DOColor(originalColor, animationDuration / 2);
                    callback?.Invoke();
                });

            return animationDuration + TweeningDelay;
        }

        public static float Enlarge(this Graphic graphic, Action callback = null, float animationDuration = 0.5f, float scaleMultiplier = 1.5f)
        {
            if (IsAnimatingDictionary.ContainsKey(graphic) && IsAnimatingDictionary[graphic])
                return animationDuration + TweeningDelay;

            IsAnimatingDictionary[graphic] = true;

            var originalScale = graphic.transform.localScale;

            graphic.transform.DOScale(originalScale * scaleMultiplier, animationDuration / 2)
                .OnComplete(() =>
                {
                    graphic.transform.DOScale(originalScale, animationDuration / 2)
                        .OnComplete(() =>
                        {
                            IsAnimatingDictionary[graphic] = false;
                            callback?.Invoke();
                        });
                });

            return animationDuration + TweeningDelay;
        }
    
        public static float WarningEnlarge(this Graphic graphic, Action callback = null, float animationDuration = 0.5f, float scaleMultiplier = 1.5f)
        {
            if (IsAnimatingDictionary.ContainsKey(graphic) && IsAnimatingDictionary[graphic])
                return animationDuration + TweeningDelay;

            IsAnimatingDictionary[graphic] = true;

            var originalScale = graphic.transform.localScale;
            var originalColor = graphic.color;
        
            graphic.transform.DOScale(originalScale * scaleMultiplier, animationDuration / 2)
                .OnStart(() => graphic.DOColor(Color.red, animationDuration / 2))
                .OnComplete(() =>
                {
                    graphic.transform.DOScale(originalScale, animationDuration / 2)
                        .OnComplete(() =>
                        {
                            graphic.DOColor(originalColor, animationDuration / 2);
                            IsAnimatingDictionary[graphic] = false;
                            callback?.Invoke();
                        });
                });

            return animationDuration + TweeningDelay;
        }


        public static float Twinkle(this Graphic graphic, Action callback = null, float singleAnimationDuration = 0.25f, float scaleMultiplier = 1.25f, float fadeMultiplier = 0.5f, int repeatCount = 3)
        {
            var originalScale = graphic.transform.localScale;
            graphic.DOFade(0f, 0f);

            var sequence = DOTween.Sequence();
            for (var i = 0; i < repeatCount; i++)
            {
                sequence.Append(graphic.transform.DOScale(originalScale * scaleMultiplier, singleAnimationDuration))
                    .Append(graphic.transform.DOScale(originalScale, singleAnimationDuration));

                sequence.Join(graphic.DOFade(fadeMultiplier, singleAnimationDuration))
                    .Join(graphic.DOFade(1f, singleAnimationDuration));
            }

            sequence.OnComplete(() =>
            {
                graphic.DOFade(0f, 0f);
                callback?.Invoke();
            });

            return sequence.Duration() + TweeningDelay;
        }
    }
}