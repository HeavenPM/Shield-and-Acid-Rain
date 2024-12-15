using Gameplay.Character;
using UnityEngine;

namespace Gameplay.Tiles
{
    public class Tile : MonoBehaviour
    {
        private TilePool _tilePool;
        private Vector2Int _gridPosition;
        private Vector2 _tileSize;

        public Vector2Int GridPosition => _gridPosition;

        public void Init(TilePool tilePool, Vector2Int position, Vector2Int mapSize)
        {
            _tilePool = tilePool;
            _gridPosition = position;
            _tileSize = GetComponent<SpriteRenderer>().bounds.size;

            transform.position = new Vector2(
                _tileSize.x * (_gridPosition.x - mapSize.x / 2), 
                _tileSize.y * (_gridPosition.y - mapSize.y / 2));
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out CharacterFacade _)) return;
        
            _tilePool.OnTileTriggered(this);
        }
    }
}
