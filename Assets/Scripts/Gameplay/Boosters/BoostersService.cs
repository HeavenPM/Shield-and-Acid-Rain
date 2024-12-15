using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.Boosters
{
    public class BoostersService : IInitializable, ITickable
    {
        [Inject] private GameService _gameService;
        
        private Dictionary<BoosterType, Booster> _boosters;
        
        public void Initialize()
        {
            _boosters = new Dictionary<BoosterType, Booster>
            {
                { BoosterType.Shield, new Booster(BoosterType.Shield, .05f) },
                { BoosterType.Boots, new Booster(BoosterType.Boots, .05f) }
            };

            _gameService.Preparing += OnGamePreparing;
        }
        
        public IReadOnlyReactiveProperty<bool> GetBoosterActiveState(BoosterType type)
            => _boosters[type].IsActive;

        public IReadOnlyReactiveProperty<float> GetBoosterStrength(BoosterType type)
            => _boosters[type].Strength;

        public void PickUpBooster(BoosterType type)
        {
            if (!_boosters.TryGetValue(type, out var booster)) return;

            booster.Strength.Value += .2f;
            booster.Strength.Value = Mathf.Min(booster.Strength.Value, 1f);
        }

        public void ToggleBooster(BoosterType type)
        {
            if (!_boosters.TryGetValue(type, out var booster)) return;
            if (booster.Strength.Value == 0f) return;

            booster.IsActive.Value = !booster.IsActive.Value;
        }

        public void Tick()
        {
            foreach (var booster in _boosters.Values)
                ProcessBooster(booster);
        }

        private void ProcessBooster(Booster booster)
        {
            if (!booster.IsActive.Value) return;

            booster.Strength.Value -= booster.FlowRate * Time.deltaTime;

            if (booster.Strength.Value > 0f) return;

            booster.Strength.Value = 0f;
            booster.IsActive.Value = false;
        }

        private void OnGamePreparing()
        {
            foreach (var booster in _boosters.Values)
            {
                booster.Strength.Value = 0f;
                booster.IsActive.Value = false;
            }
        }
    }
}