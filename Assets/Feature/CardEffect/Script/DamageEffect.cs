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


        protected void ResolveAttack(EffectContext context, out int damage, out List<CardAndHealthEntityOwnerData> targets)
        {
            int flatBonus = 0;

            targets = ResolveTargets(context);
            ApplyAttackModifiers(context, ref targets, ref flatBonus);

            int baseDamage = GetBaseValue(context);
            damage = Mathf.RoundToInt((baseDamage + flatBonus) * ResolveCritMultiplier(context));
        }


        public int CalculateBarrageDamage(EffectContext context)
        {
            List<CardAndHealthEntityOwnerData> targets = null;
            int flatBonus = 0;

            ApplyAttackModifiers(context, ref targets, ref flatBonus);

            int baseDamage = GetBaseValue(context);
            return Mathf.RoundToInt((baseDamage + flatBonus) * ResolveCritMultiplier(context));
        }

        /// <summary>Базовое значение урона до бонусов и крита. Наследник может переопределить.</summary>
        protected virtual int GetBaseValue(EffectContext context) => GetValue(context);


        private static float ResolveCritMultiplier(EffectContext context)
        {
            context.CritSystem.Resolve(context.Caster, context.GameSessionModel);
            return context.CritSystem.LastResult.Multiplier;
        }

        private static void ApplyAttackModifiers(
            EffectContext context,
            ref List<CardAndHealthEntityOwnerData> targets,
            ref int flatBonus)
        {
            foreach (var entity in context.GameSessionModel.GetAllEntityOwners())
            foreach (var passive in entity.PassiveEffects.ActivePassives.CurrentValue)
                if (passive is AttackModifierAura aura && aura.ResolveAura(entity, context.Caster, context.GameSessionModel))
                {
                    flatBonus += aura.GetDamageBonus(context.CardData);
                    targets = ModifyTargetsIfNeeded(aura, targets, context);
                }
        }

        private static List<CardAndHealthEntityOwnerData> ModifyTargetsIfNeeded(
            AttackModifierAura aura, List<CardAndHealthEntityOwnerData> targets, EffectContext context)
        {
            if (targets == null) return null;
            return aura.ModifyTargets(targets, context);
        }
    }
}
