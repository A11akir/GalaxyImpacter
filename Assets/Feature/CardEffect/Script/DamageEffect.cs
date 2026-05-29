using System;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class DamageEffect : CardEffect
    {
        public override void Execute(EffectContext context)
        {
            int damage = context.CardData.Values[context.ValueIndex];
            context.CombatSystem.TakeDamage(context.Target, damage, context.Caster, context.CardData);
        }
    }
}