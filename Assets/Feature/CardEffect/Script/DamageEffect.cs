using System;
using System.Collections.Generic;
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
            ResolveAttack(context, out int finalDamage, out var targets);

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

        /// <summary>
        /// Один проход по всем модификаторам атаки: урон (бонус + множитель) и цели.
        /// </summary>
        protected void ResolveAttack(EffectContext context, out int damage, out List<CardAndHealthEntityOwnerData> targets)
        {
            int flatBonus = 0;
            float multiplier = 1f;

            targets = ResolveTargets(context);
            ApplyAttackModifiers(context, ref targets, ref flatBonus, ref multiplier);

            int baseDamage = context.CardData.Values[context.ValueIndex];
            damage = Mathf.RoundToInt((baseDamage + flatBonus) * multiplier);
        }

        /// <summary>
        /// Только число урона, без модификации целей (для эффектов, считающих урон заранее).
        /// </summary>
        public int CalculateDamage(EffectContext context)
        {
            List<CardAndHealthEntityOwnerData> targets = null;
            int flatBonus = 0;
            float multiplier = 1f;

            ApplyAttackModifiers(context, ref targets, ref flatBonus, ref multiplier);

            int baseDamage = context.CardData.Values[context.ValueIndex];
            return Mathf.RoundToInt((baseDamage + flatBonus) * multiplier);
        }

        private static void ApplyAttackModifiers(
            EffectContext context,
            ref List<CardAndHealthEntityOwnerData> targets,
            ref int flatBonus,
            ref float multiplier)
        {
            foreach (var entity in context.GameSessionModel.GetAllEntityOwners())
            foreach (var passive in entity.PassiveEffects.ActivePassives.CurrentValue)
                if (passive is AttackModifierAura aura && aura.ResolveAura(entity, context.Caster, context.GameSessionModel))
                {
                    flatBonus += aura.GetDamageBonus(context.CardData);
                    multiplier *= aura.GetDamageMultiplier(context.CardData);
                    targets = ModifyTargetsIfNeeded(aura, targets, context);
                }
        }

        private static List<CardAndHealthEntityOwnerData> ModifyTargetsIfNeeded(
            AttackModifierAura aura, List<CardAndHealthEntityOwnerData> targets, EffectContext context)
        {
            if (targets == null) return null;   // CalculateDamage: цели не участвуют
            return aura.ModifyTargets(targets, context);
        }
    }
}
