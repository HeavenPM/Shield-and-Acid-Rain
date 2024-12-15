using UnityEngine;
using UnityEngine.UI;
using Utils;
using Zenject;

namespace Inventory
{
    public class SkinSetter : MonoBehaviour
    {
        [SerializeField] private InventoryCategory _category;
        [SerializeField] private string _spritePrefix;

        [Inject] private InventoryService _inventoryService;
        [Inject] private ResourcesLoader _resourcesLoader;

        private SpriteRenderer _spriteRenderer;
        private Image _image;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _image = GetComponent<Image>();
        }

        private void OnEnable()
        {
            UpdateView();
            _inventoryService.Updated += UpdateView;
        }

        private void OnDisable()
            => _inventoryService.Updated -= UpdateView;

        private void UpdateView()
        {
            var id = _inventoryService.GetItem(_category);
            var categoryLower = $"{_category}".ToLower();
            var sprite = _resourcesLoader
                .Get<Sprite>($"sprite_{_spritePrefix}_{categoryLower}_{id}");

            if (_spriteRenderer != null) _spriteRenderer.sprite = sprite;
            if (_image != null) _image.sprite = sprite;
        }
    }
}
