using System;
using Feature.GameSessionData;

namespace Feature.CardEffect.Script
{
    /// <summary>
    /// Наносит урон целям (TargetSelection): велью + текущая прочность щита кастера.
    /// Велью: слот 0 — базовый урон.
    /// </summary>
    [Serializable]
    public class ShieldDamageEffect : DamageEffect
    {
        public override void Execute(EffectContext context)
        {
            ResolveAttack(context, out int damage, out var targets);

            int finalDamage = damage + context.Caster.ShieldValue;

            foreach (var target in targets)
            {
                if (target == null) continue;

                context.CombatSystem.DealDamage(
                    target,
                    finalDamage,
                    context.Caster,
                    context.CardData,
                    _damageType);
            }
        }
    }
}
