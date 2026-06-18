using System;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class HealEffect : CardEffect
    {
        public override void Execute(EffectContext context)
        {
            int heal = context.CardData.Values[context.ValueIndex];
            int max = context.Target.MaxHealth;
            context.Target.HealthValue = max > 0
                ? Math.Min(context.Target.HealthValue + heal, max)
                : context.Target.HealthValue + heal;
        }
    }
}