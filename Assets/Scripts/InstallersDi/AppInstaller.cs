using AudioSlider;
using Gameplay.Camera;
using Inventory;
using UnityEngine;
using Zenject;

namespace InstallersDi
{
    public class AppInstaller : MonoInstaller
    {
        [SerializeField] private AudioPlayer _audioPlayer;
    
        public override void InstallBindings()
        {
            Container.Bind<IAudioPlayer>().FromInstance(_audioPlayer.GetComponent<IAudioPlayer>()).AsSingle();

            Container.Bind<CameraTracker>().FromInstance(Camera.main.GetComponent<CameraTracker>()).AsSingle();

            Container.BindInterfacesAndSelfTo<InventoryService>().AsSingle();
        }
    }
}
