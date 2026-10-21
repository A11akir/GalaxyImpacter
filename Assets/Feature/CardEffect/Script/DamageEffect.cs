using System;
using Feature.CombatSystem;
using Feature.GameSessionData;
using Feature.PassiveEffect.Script;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class DamageEffect : CardEffect
    {
        [SerializeField] protected DamageType _damageType = DamageType.Normal;

        public override void Execute(EffectContext context)
        {
            int finalDamage = CalculateDamage(context);
            var targets = ResolveTargets(context);

            foreach (var target in targets)
            {
                context.CombatSystem.DealDamage(
                    target,
                    finalDamage,
                    context.Caster,
                    context.CardData,
                    _damageType);
            }
        }

        public int CalculateDamage(EffectContext context)
        {
            int damage = context.CardData.Values[context.ValueIndex];
            int flatBonus = 0;
            float multiplier = 1f;

            foreach (var entity in context.GameSessionModel.GetAllEntityOwners())
            foreach (var passive in entity.PassiveEffects.ActivePassives.CurrentValue)
                if (passive is DamageBonusAura aura && aura.ResolveAura(entity, context.Caster, context.GameSessionModel))
                {
                    flatBonus += aura.GetDamageBonus(context.CardData);
                    multiplier *= aura.GetDamageMultiplier(context.CardData);
                }

            return Mathf.RoundToInt((damage + flatBonus) * multiplier);
        }
    }
}
