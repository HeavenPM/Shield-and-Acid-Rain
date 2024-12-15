using DG.Tweening;
using UnityEngine;
using WindowSystem.Runtime.Scripts;
using Zenject;

namespace Gameplay
{
    public class GameStarter : MonoBehaviour
    {
        [Inject] private GameService _gameService;
        [Inject] private WindowsManager _windowsManager;
        
        private Window _window;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _window = GetComponentInParent<Window>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }
    
        private void OnEnable()
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
            
            _window.AfterShow += OnWindowReady;
            _window.Asleep += OnWindowSleep;
            _windowsManager.WokenUp += OnWindowPreparing;
        }
    
        private void OnDisable()
        {
            _window.AfterShow -= OnWindowReady;
            _window.Asleep -= OnWindowSleep;
            _windowsManager.WokenUp -= OnWindowPreparing;
        }

        private void OnWindowSleep() => _canvasGroup.DOFade(1f, 0f).SetDelay(.5f);
        
        private void OnWindowPreparing(string name, Window window)
        {
            if (window != _window) return;
            ShowFadeAnimation(2f);
        }
        
        private void OnWindowReady()
           => ShowFadeAnimation();

        private void ShowFadeAnimation(float delay = 1f)
        {
            var startedCallback = _gameService.Prepare();
            _canvasGroup
                .DOFade(0f, 1f)
                .SetDelay(delay)
                .From(1f)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    _canvasGroup.blocksRaycasts = false;
                    startedCallback();
                });
        }
    }
}
