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
        [SerializeField] private DamageType _damageType = DamageType.Normal;
        [SerializeField] private DamageSource _damageSource = DamageSource.Value;

        public override void Execute(EffectContext context)
        {
            var targets = ResolveTargets(context);

            foreach (var target in targets)
            {
                int finalDamage = CalculateDamage(context, target);

                context.CombatSystem.DealDamage(
                    target,
                    finalDamage,
                    context.Caster,
                    context.CardData,
                    _damageType);
            }
        }

        public int CalculateDamage(EffectContext context, CardAndHealthEntityOwnerData target = null)
        {
            int damage = _damageSource switch
            {
                DamageSource.Value => context.CardData.Values[context.ValueIndex],
                DamageSource.SelfHealth => context.Caster.HealthValue,
                DamageSource.TargetHealth => target?.HealthValue ?? 0,
                _ => 0
            };

            int bonus = 0;
            foreach (var passive in context.Caster.PassiveEffects.ActivePassives.CurrentValue)
                if (passive is IDamageModifier modifier)
                    bonus += modifier.GetDamageBonus(context.CardData);

            return damage + bonus;
        }
    }
}