using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Utils
{
    public class JsonLoader
    {
        public T Get<T>(string path)
        {
            var jsonFile = Resources.Load<TextAsset>(path);
        
            if (jsonFile == null)
                throw new Exception($"The JSON file at the specified path: \"{path}\" does not exist");
        
            return JsonConvert.DeserializeObject<T>(jsonFile.text);
        }
    }
}