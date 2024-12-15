using UnityEngine;
using DG.Tweening;
using Gameplay.Boosters;
using Gameplay.Character;
using Utils;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay.Rain
{
    public class Raindrop : MonoBehaviour
    {
        private const int Damage = 1;
        private const int PuddlesTypesCount = 2;
        private const int PercentageOfPuddleGeneration = 2;
        
        [Inject] private ResourcesLoader _resourcesLoader;
        
        private float _fallDistance;
        private Vector3 _startPosition;
        private Tween _tween;
        private Puddle[] _puddles;

        public void SetFallDistance(float distance)
        {
            _fallDistance = distance;
            _startPosition = transform.position;
            StartFall();
        }

        private void Awake()
        {
            _puddles = new Puddle[PuddlesTypesCount];

            for (var i = 0; i < PuddlesTypesCount; i++)
                _puddles[i] = _resourcesLoader.Get<Puddle>($"prefab_puddle_{i}");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            CheckHitShield(other);
            CheckHitCharacter(other);
        }

        private void CheckHitShield(Collider2D other)
        {
            if (!other.TryGetComponent(out BoosterView boosterView)) return;
            if (boosterView.BoosterType != BoosterType.Shield) return;

            _tween?.Kill();

            var leftEdge = boosterView.transform.GetChild(0).position;
            var rightEdge = boosterView.transform.GetChild(1).position;

            SteerAlongShield(boosterView, leftEdge, rightEdge);
        }

        private void CheckHitCharacter(Collider2D other)
        {
            if (!other.TryGetComponent(out CharacterFacade characterFacade)) return;

            var stats = characterFacade.Stats;
            stats.ApplyDamage(Damage);
        }

        private void StartFall()
        {
            var targetPosition = _startPosition + new Vector3(0, -_fallDistance, 0);

            _tween = transform.DOMove(targetPosition, Random.Range(1f, 2f))
                .SetEase(Ease.Linear)
                .OnComplete(OnFallComplete);
        }

        private void OnFallComplete()
        {
            var randInt = Random.Range(0, 100);
            if (randInt <= PercentageOfPuddleGeneration)
            {
                var newPuddle = Instantiate(_puddles[Random.Range(0, _puddles.Length)]);
                newPuddle.transform.position = transform.position;
            }
            
            ReturnToStart();
        }

        private void ReturnToStart()
        {
            transform.position = _startPosition;
            gameObject.SetActive(false);
        }
        
        private void SteerAlongShield(BoosterView boosterView, Vector3 leftEdge, Vector3 rightEdge)
        {
            var closestEdge = Vector3.Distance(transform.position, leftEdge) < Vector3.Distance(transform.position, rightEdge)
                ? leftEdge
                : rightEdge;

            var shieldDirection = (closestEdge - transform.position).normalized;
            var shieldDistance = Vector3.Distance(transform.position, closestEdge);

            _tween = transform.DOMove(transform.position + shieldDirection * shieldDistance, 0.5f)
                .SetEase(Ease.Linear)
                .OnComplete(ReturnToStart);
        }

        private void OnDisable()
            => transform.DOKill();
    }
}
