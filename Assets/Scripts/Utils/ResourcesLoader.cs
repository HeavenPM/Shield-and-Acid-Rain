using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
    public class ResourcesLoader
    {
        private readonly ResourcesLoader _parent;
        private readonly Cache<Object> _resources = new();
    
        private Dictionary<string, AssetData> _assetsData = new();

        public ResourcesLoader(ResourcesLoader parent = null)
            => _parent = parent;
    

        public void Add(Dictionary<string, AssetData> assetInfos) 
            => _assetsData = _assetsData
                .Concat(assetInfos)
                .ToDictionary(p => p.Key, p => p.Value );

        public T[] GetMany<T>(string[] ids) where T : Object
        {
            var length = ids.Length;
            var values = new T[length];
            for (var i = 0; i < length; i++)
            {
                values[i] = Get<T>(ids[i]);
            }

            return values;
        }

        public T Get<T>(string id) where T : Object
        {
            if (IsLoaded(id)) return _resources.Get(id) as T;
            return CanLoad(id) ? Load<T>(id) : _parent?.Get<T>(id);
        }

        private T Load<T>(string id) where T : Object
        {
            var info = _assetsData[id];
            var value = Resources.Load<T>(info.Path);
            _resources.Add(id, value);

            return value;
        }

        private bool IsLoaded(string id) 
            => _resources.Contains(id);

        private bool CanLoad(string id) 
            => _assetsData.ContainsKey(id);
    }
}
