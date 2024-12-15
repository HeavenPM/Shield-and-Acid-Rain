using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace WindowSystem.Runtime.Scripts
{
    public class WindowsManager
    {
        private readonly List<Window> _history = new();
        private readonly WindowsSpawner _spawner = new();

        public event Action<string, Window> Shown, Hidden, WokenUp; 
        
        public WindowsManager(Transform container) 
            => _spawner.SetContainer(container);

        public async void ShowAlone(string id, bool canRevisit = true, Action callback = null, WindowAnimationType animationType = WindowAnimationType.NoAnimation)
        {
            Log($"start SHOW-ALONE [{id}]");

            await ShowWindow(animationType, id, canRevisit, callback);
            await HideVisibleWindows(GetWindow(id));
            
            Log($"end SHOW-ALONE {id}");
        }

        public async void ShowOver(string id, Action callback = null, WindowAnimationType animationType = WindowAnimationType.NoAnimation)
        { 
            Log($"start SHOW-OVER [{id}]");
            
            PutPreviousWindowToSleep();
            await ShowWindow(animationType, id, false, callback);
            
            Log($"end SHOW-OVER [{id}]");
        }

        public async void ShowPrevious(WindowAnimationType animationType)
        {
            Log("start SHOW-PREVIOUS");
            
            await HidePreviousWindow(animationType);
            
            var window = _history.Last();

            switch (window.State)
            {
                case WindowState.Invisible:
                    await window.Show(animationType);
                    break;
                
                case WindowState.Dormant:
                    WokenUp?.Invoke(window.name, window);
                    window.Wake();
                    break;
            }
            
            Log("start SHOW-PREVIOUS");
        }

        private async Task ShowWindow(WindowAnimationType animationType, string id, bool canRevisit, Action callback = null)
        {
            var window = GetWindow(id);
            _history.Add(window);
            Shown?.Invoke(window.name, window);
            
            await window.Show(animationType, canRevisit, callback);
        }

        private async Task Hide(Window window, bool removeFromHistory = false, Action callback = null, WindowAnimationType animationType = WindowAnimationType.NoAnimation)
        {
            Hidden?.Invoke(window.name, window);
            await window.Hide(animationType, callback);

            if (!window.CanRevisit || removeFromHistory)
            {
                Log($"remove [{window.name}] from history");
                _history.Remove(window);
            }
        }

        private async Task HideVisibleWindows(Window ignoreWindow = null)
        {
            for (var i = _history.Count - 1; i >= 0; i--)
            {
                var window = _history.ElementAt(i);
                
                if (ignoreWindow == window) continue;
                
                if (!window.isInvisible)
                {
                    await Hide(window);
                }
            }
        }

        private void PutPreviousWindowToSleep() 
            => _history.Last().Sleep();

        private Task HidePreviousWindow(WindowAnimationType animationType)
            => Hide(_history.Last(), true, null, animationType);
        
        private Window GetWindow(string id) 
            => _spawner.GetWindow(id);

        public void AddPrefab(string id, Window window) 
            => _spawner.AddPrefab(id, window);

        private void Log(string message) 
            => Debug.Log($"WindowsManager {message}");
    }
}