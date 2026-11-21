using System;
using System.Collections.Generic;
using Feature.CardEffect.Script;
using Feature.GameSessionData;
using UnityEngine;

namespace Feature.PassiveEffect.Script
{

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
        
        public override int GetDamageBonus(CardStatsData sourceCard)
        {
            int bonus = 0;

            foreach (var modifier in _modifiers)
                if (modifier is IDamageBonusModifier damageModifier)
                    bonus += damageModifier.DamageBonus;

            return bonus;
        }

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

        public override PassiveEffectBase Clone()
        {
            var clone = new TargetModifierAura
            {
                TargetSelection = TargetSelection,
                _modifiers = _modifiers
            };
            CopyBaseFields(clone);
            return clone;
        }
    }
}
