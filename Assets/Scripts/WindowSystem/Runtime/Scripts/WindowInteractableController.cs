using UnityEngine;

namespace WindowSystem.Runtime.Scripts
{
    [RequireComponent(typeof(CanvasGroup), typeof(Window))]
    public class WindowInteractableController : MonoBehaviour
    {
        private Window _window;
        private CanvasGroup _canvasGroup;
        
        private void Awake()
        {
            _window = GetComponent<Window>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            _window.BeforeShow += OnWindowBeforeShow;
            _window.AfterShow += OnWindowAfterShow;
            
            _window.BeforeHide += OnWindowBeforeHide;
            _window.AfterHide += OnWindowAfterHide;
            
            _window.Asleep += OnWindowAsleep;
            _window.WokenUp += OnWindowWokenUp;
        }
        
        private void OnDisable()
        {
            _window.BeforeShow -= OnWindowBeforeShow;
            _window.AfterShow -= OnWindowAfterShow;
            
            _window.BeforeHide -= OnWindowBeforeHide;
            _window.AfterHide -= OnWindowAfterHide;
            
            _window.Asleep -= OnWindowAsleep;
            _window.WokenUp -= OnWindowWokenUp;
        }
        
        private void OnWindowBeforeShow() 
            => _canvasGroup.interactable = false;

        private void OnWindowAfterShow() 
            => _canvasGroup.interactable = true;

        private void OnWindowBeforeHide() 
            => _canvasGroup.interactable = false;

        private void OnWindowAfterHide() 
            => _canvasGroup.interactable = true;

        private void OnWindowAsleep() 
            => _canvasGroup.interactable = false;

        private void OnWindowWokenUp() 
            => _canvasGroup.interactable = true;
    }
}