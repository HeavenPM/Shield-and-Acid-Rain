using UnityEngine;
using Utils;
using WindowSystem.Runtime.Scripts;
using Zenject;

namespace WindowSystem.Controllers
{
    public class WindowRouter : UIController
    {
        [SerializeField] private string _id;
        [SerializeField] private WindowAnimationType _animationType;
    
        [Inject] private WindowsManager _windows;

        public override void HandleListener()
            => _windows.ShowAlone(_id, animationType: _animationType);
    }
}