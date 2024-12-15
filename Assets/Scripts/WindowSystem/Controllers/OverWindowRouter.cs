using UnityEngine;
using Utils;
using WindowSystem.Runtime.Scripts;
using Zenject;

namespace WindowSystem.Controllers
{
    public class OverWindowRouter: UIController
    {
        [SerializeField] private string _id;
        [SerializeField] private WindowAnimationType _animationType;
        
        [Inject] private WindowsManager _windows;
    
        public override void HandleListener()
            => _windows.ShowOver(_id, animationType: _animationType);
    }
}