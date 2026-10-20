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
            int bonus = 0;

            // Ауры: проходим по всем сущностям и смотрим, чья аура накрывает кастера.
            foreach (var entity in context.GameSessionModel.GetAllEntityOwners())
            foreach (var passive in entity.PassiveEffects.ActivePassives.CurrentValue)
                if (passive is DamageBonusAura aura && aura.Covers(context.Caster))
                    bonus += aura.GetDamageBonus(context.CardData);

            return damage + bonus;
        }
    }
}
