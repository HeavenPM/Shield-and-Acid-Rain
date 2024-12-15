using UnityEngine;
using Zenject;

namespace Inventory
{
    public class ItemPreview : MonoBehaviour
    {
        [SerializeField] private InventoryCategory _category;
        
        [Inject] private InventoryService _inventoryService;

        public InventoryCategory Category => _category;
        
        public void OnPipetteDropped(string colorId)
            => _inventoryService.SetItem(_category, colorId);
    }
}