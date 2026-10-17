using System;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class DealDamageLethalTrigger : DamageEffect
    {
        [SerializeReference] private CardEffect _onLethalEffect;

        public override void Execute(EffectContext context)
        {
            int damage = CalculateDamage(context);

            foreach (var target in ResolveTargets(context))
            {
                bool isLethal = context.CombatSystem.DealDamage(
                    target,
                    damage,
                    context.Caster,
                    context.CardData,
                    _damageType);

                if (!isLethal || _onLethalEffect == null) continue;

                ExecuteInSlot(context, 1, ctx =>
                {
                    ctx.Target = target;
                    _onLethalEffect.Execute(ctx);
                });
            }
        }
    }
}
