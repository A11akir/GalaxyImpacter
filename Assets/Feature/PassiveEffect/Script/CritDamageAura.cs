using System;
using Feature.CardEffect.Script;

namespace Feature.PassiveEffect.Script
{
  
    [Serializable]
    public class CritDamageAura : AttackModifierAura
    {
        public int ChancePercent { get; private set; }
        public int DamagePercent { get; private set; }

        public override void OnAppliedFromCard(EffectContext context)
        {
            DamagePercent = context.Value();
            ChancePercent = context.Value(1);
        }

        public override PassiveEffectBase Clone()
        {
            var clone = new CritDamageAura { TargetSelection = TargetSelection };
            CopyBaseFields(clone);
            return clone;
        }
    }
}
