using System;
using System.Collections.Generic;
using Feature.CardEffect.Script;
using Feature.GameSessionData;
using UnityEngine;

namespace Feature.PassiveEffect.Script
{
    /// <summary>
    /// Аура модификаторов целей атаки: пока живёт на сущности, её модификаторы
    /// применяются к целям урона кастеров в области TargetSelection.
    /// Велью: слот 0 = количество срабатываний (0 = без лимита),
    /// слоты 1+m = велью вложенных модификаторов по порядку.
    /// </summary>
    [Serializable]
    public class TargetModifierAura : AttackModifierAura
    {
        [SerializeReference] private List<ITargetModifier> _modifiers = new();

        private CardAndHealthEntityOwnerData _owner;
        private int _charges;

        public TargetModifierAura()
        {
            TargetSelection = TargetSelectionType.Self;
        }

        public override void OnAppliedFromCard(EffectContext context)
        {
            int saved = context.ValueIndex;

            _charges = context.CardData.Values[saved];

            for (int m = 0; m < _modifiers.Count; m++)
            {
                var modifier = _modifiers[m];
                if (modifier == null) continue;

                context.ValueIndex = saved + 1 + m;
                modifier.ReadValue(context);
            }

            context.ValueIndex = saved;
        }

        public override void Register(CardAndHealthEntityOwnerData owner) => _owner = owner;

        public override List<CardAndHealthEntityOwnerData> ModifyTargets(
            List<CardAndHealthEntityOwnerData> targets, EffectContext context)
        {
            foreach (var modifier in _modifiers)
            {
                if (modifier == null) continue;
                targets = modifier.ModifyTargets(targets, context);
            }

            SpendOneCharge();

            return targets;
        }

        private void SpendOneCharge()
        {
            if (ChargesUnlimited) return;

            _charges -= 1;

            if (_charges == 0)
                _owner?.PassiveEffects.Remove(this);
        }

        private bool ChargesUnlimited => _charges == 0;

        public override PassiveEffectBase Clone() =>
            new TargetModifierAura
            {
                Config = Config,
                Duration = Duration,
                TargetSelection = TargetSelection,
                _modifiers = _modifiers
            };
    }
}
