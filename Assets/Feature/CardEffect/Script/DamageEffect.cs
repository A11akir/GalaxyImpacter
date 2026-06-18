using System;
using Feature.PassiveEffect.Script;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class DamageEffect : CardEffect
    {
        public override void Execute(EffectContext context)
        {
            int damage = context.CardData.Values[context.ValueIndex];

            foreach (var passive in context.Caster.PassiveEffects.ActivePassives.CurrentValue)
                if (passive is IDamageModifier modifier)
                    damage += modifier.GetDamageBonus(context.CardData);

            context.CombatSystem.TakeDamage(context.Target, damage, context.Caster, context.CardData);
        }
    }
}