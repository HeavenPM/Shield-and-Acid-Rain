using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public class Cache<T>
    {
        private readonly Dictionary<string, T> _all = new();
        
        public bool Contains(string id) 
            => _all.ContainsKey(id);

        public void Add(string id, T obj) 
            => _all.Add(id, obj);

        public T Get(string id) 
            => _all[id];
        
        public List<K> Get<K>() where K : class, T 
            => _all.Values.OfType<K>().ToList();

        public void Remove(string id) 
            => _all.Remove(id);
        
        public void Clear() 
            => _all.Clear();
        
        public List<T> ValuesToList 
            => _all.Values.ToList();
        
        public T[] ValuesToArray 
            => _all.Values.ToArray();
    }
}