using System;
using Feature.PassiveEffect.Script;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class DamageEffect : CardEffect
    {
        public override void Execute(EffectContext context)
        {
           
            int finalDamage = CalculateDamage(context);

            var targets = ResolveTargets(context);
            
            Debug.Log(targets[0]._heroName);

            foreach (var target in targets)
            {
                context.CombatSystem.TakeDamage(
                    target,
                    finalDamage,
                    context.Caster,
                    context.CardData
                );
            }
        }
        
        public int CalculateDamage(EffectContext context)
        {
            int damage = context.CardData.Values[context.ValueIndex];

            int bonus = 0;

            foreach (var passive in context.Caster.PassiveEffects.ActivePassives.CurrentValue)
            {
                if (passive is IDamageModifier modifier)
                    bonus += modifier.GetDamageBonus(context.CardData);
            }

            return damage + bonus;
        }
    }
}