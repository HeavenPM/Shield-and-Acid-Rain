using UnityEngine;
using Utils;
using WindowSystem.Runtime.Scripts;
using Zenject;

namespace WindowSystem.Controllers
{
    public class BackWindowRouter : UIController
    {
        [SerializeField] private WindowAnimationType _animationType;
    
        [Inject] private WindowsManager _windows;
    
        public override void HandleListener()
            => _windows.ShowPrevious(_animationType);
    }
}