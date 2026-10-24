using System;
using Feature.CardEffect.Script;
using R3;
using UnityEngine;

namespace Feature.PassiveEffect.Script
{
    /// <summary>
    /// Плоский бонус к урону. Цели и множитель не трогает.
    /// </summary>
    [Serializable]
    public class DamageBonusAura : AttackModifierAura, IValueProvider, IStackablePassive
    {
        private readonly ReactiveProperty<int> _bonus = new(0);
        public ReadOnlyReactiveProperty<int> Value => _bonus;

        public DamageBonusAura()
        {
            TargetSelection = TargetSelectionType.Allies;
        }

        public override void OnAppliedFromCard(EffectContext context) =>
            AddBonus(context.CardData.Values[context.ValueIndex]);

        public void AddBonus(int amount) => _bonus.Value += amount;

        public override int GetDamageBonus(CardStatsData sourceCard) => _bonus.Value;

        public override PassiveEffectBase Clone() =>
            new DamageBonusAura { Config = Config, Duration = Duration, TargetSelection = TargetSelection };
    }
}
