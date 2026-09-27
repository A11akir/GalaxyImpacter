using System;
using Feature.CardEffect.Script;
using Feature.CombatSystem;
using Feature.GameSessionData;
using R3;

namespace Feature.PassiveEffect.Script
{
    [Serializable]
    public class DodgeChancePassive : PassiveEffectBase, ICardContextConsumer, IValueProvider, IDamageReaction
    {
        private readonly ReactiveProperty<int> _dodgeChancePercent = new(0);
        public ReadOnlyReactiveProperty<int> Value => _dodgeChancePercent;

        public void OnAppliedFromCard(EffectContext context)
        {
            _dodgeChancePercent.Value = context.CardData.Values[context.ValueIndex];
        }

        public override void Register(CardAndHealthEntityOwnerData owner) { }
        public override void Unregister() { }

        public int Priority => 0;

        public bool ReactToDamage(CardAndHealthEntityOwnerData target, int finalDamage, CardAndHealthEntityOwnerData source)
        {
            int roll = UnityEngine.Random.Range(0, 100);
            bool dodged = roll < _dodgeChancePercent.Value;
            return dodged; // true — урон полностью проигнорирован (уворот сработал)
        }

        public override PassiveEffectBase Clone() =>
            new DodgeChancePassive { Config = Config, Duration = Duration };
    }
}