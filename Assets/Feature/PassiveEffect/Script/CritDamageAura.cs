using System;
using Feature.CardEffect.Script;
using Feature.GameSessionData;
using Feature.Hero;
using UnityEngine;

namespace Feature.PassiveEffect.Script
{
    [Serializable]
    public class CritDamageAura : AttackModifierAura
    {
        public int ChancePercent { get; private set; }
        public int DamagePercent { get; private set; }

        public override void OnAppliedFromCard(EffectContext context)
        {
            ChancePercent = context.Value();
            DamagePercent = context.Value(1);
        }
        
        public bool ResolveAura(
            CardAndHealthEntityOwnerData owner,
            CardAndHealthEntityOwnerData caster,
            GameSessionModel model,
            CardStatsData sourceCard) =>
            IsCritCard(sourceCard) && ResolveAura(owner, caster, model);

        private static bool IsCritCard(CardStatsData sourceCard) =>
            sourceCard.Specialization.Contains(AllHeroClass.Warrior) ||
            sourceCard.Specialization.Contains(AllHeroClass.Dragonborn);

        public override PassiveEffectBase Clone()
        {
            var clone = new CritDamageAura { TargetSelection = TargetSelection };
            CopyBaseFields(clone);
            return clone;
        }
    }
}
