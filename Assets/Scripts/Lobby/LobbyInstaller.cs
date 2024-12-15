using Inventory;
using UnityEngine;
using Zenject;

namespace Lobby
{
    public class LobbyInstaller : MonoInstaller
    {
        [SerializeField] private Alert _settingsAlert;
        [SerializeField] private Pipette[] _pipettes;
    
        public override void InstallBindings()
        {
            Container.Bind<Alert>().FromInstance(_settingsAlert).AsSingle();
            
            Container.Bind<Pipette[]>().FromInstance(_pipettes).AsSingle();
        }
    }
}
