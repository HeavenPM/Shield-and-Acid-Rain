using System.Collections.Generic;
using UnityEngine;

namespace WindowSystem.Runtime.Scripts
{
    public class WindowsSpawner
    {
        private readonly Dictionary<string, Window> _prefabs = new();
        private readonly Dictionary<string, Window> _cache = new();
    
        private Transform _container;

        private Window InstantiateWindow(string prefabId)
        {
            var prefab = _prefabs[prefabId];
            prefab.gameObject.SetActive(false);
           
            var window = Object.Instantiate(prefab);
            window.SetParent(_container);
            
            prefab.gameObject.SetActive(true);

            _cache.Add(prefabId, window);
            return window;
        }

        public void AddPrefab(string id, Window window)
        {
            Debug.Log($"AddPrefab / id = {id}");
            _prefabs.Add(id, window);
        }

        public void SetContainer(Transform container) 
            => _container = container;

        public Window GetWindow(string id) 
            => _cache.ContainsKey(id) ? _cache[id] : InstantiateWindow(id);
    }
}