using System;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class PureDamageEffect : CardEffect
    {
        public override void Execute(EffectContext context)
        {
            int damage = context.CardData.Values[context.ValueIndex];

            var targets = ResolveTargets(context);

            foreach (var target in targets)
            {
                context.CombatSystem.TakePureDamage(
                    target,
                    damage,
                    context.Caster,
                    context.CardData
                );
            }
        }
    }
}