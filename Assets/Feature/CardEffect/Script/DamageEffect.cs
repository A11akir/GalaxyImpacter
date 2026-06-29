// DamageEffect.cs — добавляем выбор типа урона
using System;
using Feature.CombatSystem;
using Feature.PassiveEffect.Script;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class DamageEffect : CardEffect
    {
        [SerializeField] private DamageType _damageType = DamageType.Normal; // ← выпадающий список в инспекторе

        public override void Execute(EffectContext context)
        {
            int finalDamage = CalculateDamage(context);
            var targets = ResolveTargets(context);

            foreach (var target in targets)
            {
                context.CombatSystem.DealDamage( // ← переименован из TakeDamage
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

            foreach (var passive in context.Caster.PassiveEffects.ActivePassives.CurrentValue)
                if (passive is IDamageModifier modifier)
                    bonus += modifier.GetDamageBonus(context.CardData);

            return damage + bonus;
        }
    }
}