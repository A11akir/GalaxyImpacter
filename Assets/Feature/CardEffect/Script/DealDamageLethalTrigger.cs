using System;
using System.Collections.Generic;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class DealDamageLethalTrigger : DamageEffect
    {
        [SerializeReference] private List<CardEffect> _onLethalEffects = new();

        public override void Execute(EffectContext context)
        {
            ResolveAttack(context, out int damage, out var targets);

            foreach (var target in targets)
            {
                bool isLethal = context.CombatSystem.DealDamage(
                    target,
                    damage,
                    context.Caster,
                    context.CardData,
                    _damageType);

                if (!isLethal) continue;

                for (int i = 0; i < _onLethalEffects.Count; i++)
                {
                    var effect = _onLethalEffects[i];
                    if (effect == null) continue;

                    ExecuteInSlot(context, 1 + i, ctx =>
                    {
                        ctx.Target = target;
                        effect.Execute(ctx);
                    });
                }
            }
        }
    }
}
