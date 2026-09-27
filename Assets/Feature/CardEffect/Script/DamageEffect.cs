using System;
using System.Collections.Generic;
using System.Linq;
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

            foreach (var passive in context.Caster.PassiveEffects.ActivePassives.CurrentValue)
                if (passive is IDamageModifier modifier)
                    bonus += modifier.GetDamageBonus(context.CardData);

            foreach (var ally in GetAllies(context.Caster, context.GameSessionModel))
            foreach (var passive in ally.PassiveEffects.ActivePassives.CurrentValue)
                if (passive is ITeamDamageModifier teamModifier)
                    bonus += teamModifier.GetDamageBonus(context.CardData);

            return damage + bonus;
        }
    
        private List<CardAndHealthEntityOwnerData> GetAllies(CardAndHealthEntityOwnerData caster, GameSessionModel gameSessionModel)
        {
            var casterSide = gameSessionModel.GetPlayerDataByOwner(caster);
            return casterSide != null ? casterSide.AllAlive.ToList() : new List<CardAndHealthEntityOwnerData>();
        }
    }
}