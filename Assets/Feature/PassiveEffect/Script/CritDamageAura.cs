using System;
using System.Collections.Generic;
using Feature.CardEffect.Script;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Feature.PassiveEffect.Script
{
    /// <summary>
    /// Крит: своё велью = процент урона (150 = ×1.5). Вложенные ICritModifier
    /// читают следующие велью (как IGeneratedCardModifier у генератора карт):
    /// CritChanceModifier — шанс, CritDamageModifier — доп. урон.
    /// </summary>
    [Serializable]
    public class CritDamageAura : DamageBonusAura
    {
        [SerializeReference] private List<ICritModifier> _modifiers = new();

        private CritState _state = new();

        public override void OnAppliedFromCard(EffectContext context)
        {
            base.OnAppliedFromCard(context);   // своё велью = процент урона

            int saved = context.ValueIndex;
            for (int m = 0; m < _modifiers.Count; m++)
            {
                var modifier = _modifiers[m];
                if (modifier == null) continue;

                context.ValueIndex = saved + 1 + m;
                modifier.Apply(_state, context);
            }
            context.ValueIndex = saved;
        }

        public override int GetDamageBonus(CardStatsData sourceCard) => 0;

        public override float GetDamageMultiplier(CardStatsData sourceCard) =>
            Random.Range(0, 100) < _state.ChancePercent
                ? (Value.CurrentValue + _state.DamagePercent) / 100f
                : 1f;

        public override PassiveEffectBase Clone() =>
            new CritDamageAura
            {
                Config = Config,
                Duration = Duration,
                TargetSelection = TargetSelection,
                _modifiers = _modifiers
            };
    }
}
