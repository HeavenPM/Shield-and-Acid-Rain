using UniRx;

namespace Gameplay.Boosters
{
    public class Booster
    {
        public BoosterType Type { get; }
        public readonly ReactiveProperty<bool> IsActive = new();
        public readonly ReactiveProperty<float> Strength = new();
        public readonly float FlowRate;

        public Booster(BoosterType type, float flowRate)
        {
            Type = type;
            FlowRate = flowRate;
        }
    }
}