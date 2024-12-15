using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
using Zenject;

namespace Gameplay.Tiles
{
    public class TilePool : MonoBehaviour
    {
        private readonly Vector2Int _mapSize = new(5, 5);
        private readonly List<Tile> _templates = new();

        [Inject] private GameService _gameService;
        [Inject] private ResourcesLoader _resourcesLoader;

        private List<Tile> _tileMap;
        private Tile _lastTriggeredTile;

        public void OnTileTriggered(Tile triggeredTile)
        {
            if (_lastTriggeredTile == null)
            {
                _lastTriggeredTile = triggeredTile;
                return;
            }

            if (_lastTriggeredTile.GridPosition.x != triggeredTile.GridPosition.x)
            {
                AddColumn(fromLeft: triggeredTile.GridPosition.x < _lastTriggeredTile.GridPosition.x);
                RemoveColumn(fromLeft: triggeredTile.GridPosition.x > _lastTriggeredTile.GridPosition.x);
            }

            if (_lastTriggeredTile.GridPosition.y != triggeredTile.GridPosition.y)
            {
                AddRow(fromTop: triggeredTile.GridPosition.y > _lastTriggeredTile.GridPosition.y);
                RemoveRow(fromTop: triggeredTile.GridPosition.y < _lastTriggeredTile.GridPosition.y);
            }

            _lastTriggeredTile = triggeredTile;
        }

    
        private void Awake()
        {
            LoadTemplates();
        }

        private void OnEnable()
        {
            OnGamePreparing();
            _gameService.Preparing += OnGamePreparing;
        }

        private void OnDisable()
        {
            _gameService.Preparing -= OnGamePreparing;
        }

        private void OnGamePreparing()
        {
            ClearAll();
        
            _lastTriggeredTile = null;
            StartCoroutine(GenerateMapCoroutine());
        }

        private IEnumerator GenerateMapCoroutine()
        {
            for (var y = 0; y < _mapSize.y; y++)
            {
                for (var x = 0; x < _mapSize.x; x++)
                {
                    yield return null;

                    var newTile = Instantiate(_templates[Random.Range(0, _templates.Count)], transform);
                    newTile.Init(this, new Vector2Int(x, y), _mapSize);
                    _tileMap.Add(newTile);
                }
            }
        }
        
        private void ClearAll()
        {
            _tileMap = new();
        
            foreach (Transform child in transform)
                Destroy(child.gameObject);
        }

        private void LoadTemplates()
        {
            const int tilesCount = 7;

            for (var i = 0; i < tilesCount; i++)
            {
                var tile = _resourcesLoader.Get<Tile>($"prefab_tile_{i}");
                _templates.Add(tile);
            }
        }
        
        private void AddRow(bool fromTop)
        {
            var newTiles = new List<Tile>();

            var newY = fromTop
                ? _tileMap.Max(t => t.GridPosition.y) + 1
                : _tileMap.Min(t => t.GridPosition.y) - 1;

            var existingX = _tileMap.Select(t => t.GridPosition.x).Distinct().ToList();

            foreach (var x in existingX)
            {
                var newTile = Instantiate(_templates[Random.Range(0, _templates.Count)], transform);
                var newPosition = new Vector2Int(x, newY);

                newTile.Init(this, newPosition, _mapSize);
                newTiles.Add(newTile);
            }

            _tileMap.AddRange(newTiles);
        }

        private void RemoveRow(bool fromTop)
        {
            var targetY = fromTop
                ? _tileMap.Max(t => t.GridPosition.y)
                : _tileMap.Min(t => t.GridPosition.y);

            var tilesToRemove = _tileMap.Where(t => t.GridPosition.y == targetY).ToList();

            foreach (var tile in tilesToRemove)
            {
                Destroy(tile.gameObject);
                _tileMap.Remove(tile);
            }
        }

        private void AddColumn(bool fromLeft)
        {
            var newTiles = new List<Tile>();

            var newX = fromLeft
                ? _tileMap.Min(t => t.GridPosition.x) - 1
                : _tileMap.Max(t => t.GridPosition.x) + 1;

            var referenceTiles = _tileMap.Where(t => t.GridPosition.x == _tileMap[0].GridPosition.x).ToList();
            foreach (var tile in referenceTiles)
            {
                var newTile = Instantiate(_templates[Random.Range(0, _templates.Count)], transform);
                var newPosition = new Vector2Int(newX, tile.GridPosition.y);

                newTile.Init(this, newPosition, _mapSize);
                newTiles.Add(newTile);
            }

            _tileMap.AddRange(newTiles);
        }

        private void RemoveColumn(bool fromLeft)
        {
            var targetX = fromLeft
                ? _tileMap.Min(t => t.GridPosition.x)
                : _tileMap.Max(t => t.GridPosition.x);

            var tilesToRemove = _tileMap.Where(t => t.GridPosition.x == targetX).ToList();

            foreach (var tile in tilesToRemove)
            {
                Destroy(tile.gameObject);
                _tileMap.Remove(tile);
            }
        }
    }
}