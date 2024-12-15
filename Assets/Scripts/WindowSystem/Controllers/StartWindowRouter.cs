using UnityEngine;
using WindowSystem.Runtime.Scripts;
using Zenject;

namespace WindowSystem.Controllers
{
    public class StartWindowRouter : MonoBehaviour
    {
        [SerializeField] private string _screenId = "screen_lobby";
    
        [Inject] private WindowsManager _windows;
    
        private void Start() 
            => _windows.ShowAlone(_screenId);
    }
}