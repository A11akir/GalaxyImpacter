using System;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class DealDamageEquipShieldEffect : DamageEffect
    {
        public override void Execute(EffectContext context)
        {
            ResolveAttack(context, out int damage, out var targets);

            foreach (var target in targets)
            {
                context.CombatSystem.DealDamage(
                    target,
                    damage,
                    context.Caster,
                    context.CardData,
                    _damageType);
                
                target.SetShield(damage);
            }
        }
    }
}
