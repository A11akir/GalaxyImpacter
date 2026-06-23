using System;
using Feature.Card.Script;
using Feature.CardEffect.Script;
using Feature.GameSessionData;
using Feature.Hero;
using R3;

namespace Feature.PassiveEffect.Script
{
    [Serializable]
    public class FireDamageBonus : PassiveEffectBase, IDamageModifier, IStackablePassive, IValueProvider, ICardContextConsumer
    {
        private int _bonus;
        private readonly ReactiveProperty<int> _value = new(0);

        public void OnAppliedFromCard(EffectContext context) =>
            AddBonus(context.CardData.Values[context.ValueIndex]);
        public ReadOnlyReactiveProperty<int> Value => _value;

        public FireDamageBonus()
        {
            Duration = DurationType.UntilTurnEnd;
        }

        public void AddBonus(int amount)
        {
            _bonus += amount;
            _value.Value = _bonus;
        }

        public override void Register(CardAndHealthEntityOwnerData owner, CombatSystem.CombatSystem combatSystem, CardCastService cardCastService, CardPoolPickSystem cardPoolPickSystem) { }
        public override void Unregister() { }

        public int GetDamageBonus(CardStatsData sourceCard)
        {
            if (!sourceCard) return 0;
            if (!sourceCard.Specialization.Contains(AllHeroClass.FireMage)) return 0;
            return _bonus;
        }

        public override PassiveEffectBase Clone() => new FireDamageBonus { Config = Config, Duration = Duration };
    }
}