// FireDamageBonus.cs — теперь только про данные, не про "когда умирать"

using System;
using Feature.CardEffect.Script;
using Feature.GameSessionData;
using Feature.Hero;
using R3;

namespace Feature.PassiveEffect.Script
{
    [Serializable]
    public class FireDamageBonus : PassiveEffect, IDamageModifier, IStackablePassive, IValueProvider
    {
        private int _bonus;
        private readonly ReactiveProperty<int> _value = new(0);

        public ReadOnlyReactiveProperty<int> Value => _value;

        public FireDamageBonus()
        {
            Duration = DurationType.UntilTurnEnd; // ← явно декларирует своё время жизни
        }

        public void AddBonus(int amount)
        {
            _bonus += amount;
            _value.Value = _bonus;
        }

        public override void Register(CardAndHealthEntityOwnerData owner, CombatSystem.CombatSystem combatSystem) { }
        public override void Unregister() { }

        protected override void OnTurnTick()
        {
            _bonus = 0;
            _value.Value = 0;
        }

        public int GetDamageBonus(CardStatsData sourceCard)
        {
            if (!sourceCard) return 0;
            if (!sourceCard.Specialization.Contains(AllHeroClass.FireMage)) return 0;
            return _bonus;
        }

        public override PassiveEffect Clone() => new FireDamageBonus { Config = Config, Duration = Duration };
    }
}