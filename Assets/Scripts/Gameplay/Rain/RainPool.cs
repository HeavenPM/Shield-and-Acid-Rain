using System.Collections;
using System.Linq;
using Gameplay.Character;
using UniRx;
using UnityEngine;
using Utils;
using Zenject;

namespace Gameplay.Rain
{
    public class RainPool : MonoBehaviour
    {
        private const int PoolSize = 1000;
        private const float RainAreaWidth = 40f;
        private const float RainSpawnHeight = 15f;
        private const float MinFallDistance = 10f;
        private const float MaxFallDistance = 40f;

        private readonly CompositeDisposable _disposables = new();

        [Inject] private RainService _rainService;
        [Inject] private ResourcesLoader _resourcesLoader;
        [Inject] private CharacterFacade _character;

        private Raindrop[] _raindrops;
        private Coroutine _coroutine;
        private Transform _characterTransform;
        private bool _isRainy;

        private void Awake()
        {
            _characterTransform = _character.transform;
            var raindropPrefab = _resourcesLoader.Get<Raindrop>("prefab_raindrop");

            _raindrops = Enumerable.Range(0, PoolSize)
                .Select(_ =>
                {
                    var instance = Instantiate(raindropPrefab, transform);
                    instance.gameObject.SetActive(false);
                    return instance;
                })
                .ToArray();
        }

        private void OnEnable()
        {
            _rainService.IsRainyWeather
                .StartWith(_rainService.IsRainyWeather.Value)
                .Subscribe(OnIsRainyWeatherUpdated)
                .AddTo(_disposables);
        }

        private void OnDisable()
        {
            _disposables.Clear();
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
            DeactivateAllRaindrops();
        }

        private void OnIsRainyWeatherUpdated(bool isRainy)
        {
            _isRainy = isRainy;

            if (!_isRainy)
            {
                if (_coroutine != null)
                {
                    StopCoroutine(_coroutine);
                    _coroutine = null;
                }
                DeactivateAllRaindrops();
                return;
            }
            _coroutine ??= StartCoroutine(RainCoroutine());
        }

        private IEnumerator RainCoroutine()
        {
            var activeRaindropIndex = 0;

            while (_isRainy)
            {
                for (var i = 0; i < 10; i++)
                {
                    var raindrop = _raindrops[activeRaindropIndex];

                    if (!raindrop.gameObject.activeSelf)
                    {
                        var spawnPosition = GetRandomSpawnPosition();
                        raindrop.transform.position = spawnPosition;
                        raindrop.SetFallDistance(Random.Range(MinFallDistance, MaxFallDistance));
                        raindrop.gameObject.SetActive(true);
                    }

                    activeRaindropIndex = (activeRaindropIndex + 1) % PoolSize;
                }

                yield return new WaitForSeconds(0.1f);
            }
        }

        private Vector3 GetRandomSpawnPosition()
        {
            var x = Random.Range(
                _characterTransform.position.x - RainAreaWidth / 2,
                _characterTransform.position.x + RainAreaWidth / 2
            );

            var y = _characterTransform.position.y + RainSpawnHeight;
            return new Vector3(x, y, 0f);
        }

        private void DeactivateAllRaindrops()
        {
            foreach (var raindrop in _raindrops)
                raindrop.gameObject.SetActive(false);
        }
    }
}
