using System;
using UnityEngine;

namespace Feature.CardEffect.Script
{

    [Serializable]
    public class DealDamageCritTrigger : DamageEffect
    {
        [SerializeReference] private CardEffect _onCritEffect;

        public override void Execute(EffectContext context)
        {
            ResolveAttack(context, out int damage, out var targets);

            bool isCrit = context.CritSystem.LastResult.IsCrit;

            foreach (var target in targets)
            {
                context.CombatSystem.DealDamage(
                    target,
                    damage,
                    context.Caster,
                    context.CardData,
                    _damageType);
            }

            if (!isCrit || _onCritEffect == null) return;

            ExecuteInSlot(context, 1, ctx => _onCritEffect.Execute(ctx));
        }
    }
}
