using System.Collections.Generic;
using UnityEngine;
using Utils;
using WindowSystem.Runtime.Scripts;
using Zenject;

namespace InstallersDi
{
    public class FactoryInstaller : MonoInstaller
    {
        [SerializeField] private Transform _windowsContainer;

        public override void InstallBindings()
        {
            var jsonLoader = new JsonLoader();
            Container.BindInstance(jsonLoader).AsSingle();
        
            var assetsData = jsonLoader.Get<Dictionary<string, AssetData>>("Configs/AssetsConfig");
            var resourceLoader = new ResourcesLoader();
            resourceLoader.Add(assetsData);
            Container.BindInstance(resourceLoader).AsSingle();
        
            var windows = new WindowsManager(_windowsContainer);
            Container.BindInstance(windows).AsSingle();

            var windowsData = jsonLoader.Get<Dictionary<string, AssetData>>("Configs/WindowsConfig");

            foreach (var data in windowsData)
            {
                var window = Resources.Load<Window>(data.Value.Path);
                windows.AddPrefab(data.Key, window);
            }

            resourceLoader.Add(windowsData);
        }
    }
}
