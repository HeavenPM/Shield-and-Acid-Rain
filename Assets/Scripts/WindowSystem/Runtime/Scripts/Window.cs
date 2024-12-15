using System;
using System.Threading.Tasks;
using UnityEngine;

namespace WindowSystem.Runtime.Scripts
{
    [DisallowMultipleComponent]
    public class Window : MonoBehaviour
    {
        public event Action BeforeShow, AfterShow;
        public event Action BeforeHide, AfterHide;
        public event Action Asleep, WokenUp;
    
        public bool CanRevisit { get; private set; }
        public WindowState State { get; private set; }

        private WindowAnimator _animator;

        private void Awake() 
            => _animator = GetComponentInChildren<WindowAnimator>();

        public async Task Show(WindowAnimationType animationType = WindowAnimationType.NoAnimation, bool canRevisit = true, Action callback = null)
        {
            Log($"start SHOW [{name}]");
        
            MoveToFront();

            BeforeShow?.Invoke();
            await _animator.Play(animationType);
        
            this.CanRevisit = canRevisit;
            State = WindowState.Visible;
        
            Log($"end SHOW [{name}]");
            
            AfterShow?.Invoke();
            callback?.Invoke();
        }
    
        public async Task Hide(WindowAnimationType animationType = WindowAnimationType.NoAnimation, Action callback = null)
        {
            Log($"start HIDE [{name}]");
        
            BeforeHide?.Invoke();
            await _animator.Play(animationType);
        
            State = WindowState.Invisible;
        
            Log($"end HIDE [{name}]");

            AfterHide?.Invoke();
            callback?.Invoke();

            MoveToBack();
        }

        public void Sleep()
        {
            Log($"{name} Sleep");
        
            State = WindowState.Dormant;
            Asleep?.Invoke();
        }

        public void Wake()
        {
            Log($"{name} Wake");
        
            State = WindowState.Visible;
            WokenUp?.Invoke();
        }
    
        private void MoveToFront()
        {
            transform.SetAsLastSibling();
            gameObject.SetActive(true);
        }
    
        private void MoveToBack()
        {
            transform.SetAsFirstSibling();
            gameObject.SetActive(false);
        }

        public void SetParent(Transform parent) 
            => transform.SetParent(parent);
    
        public bool isVisible
            => State == WindowState.Visible;
    
        public bool isDormant
            => State == WindowState.Dormant;
    
        public bool isInvisible
            => State == WindowState.Invisible;

        private void Log(string message)
            => Debug.Log($"---- Window {name} {message}");
    }
}