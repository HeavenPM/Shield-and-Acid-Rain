using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace Inventory
{
    public class InventoryService : IInitializable
    {
        private const string InventoryKey = nameof(InventoryKey);
        
        private Dictionary<InventoryCategory, string> _selectedItems;

        public event Action Updated;
        
        public void Initialize()
            => Load();

        public string GetItem(InventoryCategory category) 
            => _selectedItems[category];

        public void SetItem(InventoryCategory category, string newItem)
        {
            if (!_selectedItems.ContainsKey(category)) 
                throw new ArgumentException();
            
            _selectedItems[category] = newItem;
            Updated?.Invoke();
            Save();
        }
        
        private void Load()
        {
            var json = PlayerPrefs.GetString(InventoryKey);

            if (string.IsNullOrEmpty(json))
            {
                _selectedItems = new Dictionary<InventoryCategory, string>()
                {
                    { InventoryCategory.Shield, "red" },
                    { InventoryCategory.Boots, "red" }
                };
                Save();
                return;
            }

            _selectedItems = JsonConvert.DeserializeObject<Dictionary<InventoryCategory, string>>(json);
        }

        private void Save()
        {
            var json = JsonConvert.SerializeObject(_selectedItems);
            PlayerPrefs.SetString(InventoryKey, json);
        }
    }
}