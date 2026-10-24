using System;
using System.Collections.Generic;
using Feature.CardEffect.Script;
using Feature.GameSessionData;
using UnityEngine;

namespace Feature.PassiveEffect.Script
{
    /// <summary>
    /// Базовый модификатор атаки: бонус и множитель урона + модификация целей.
    /// Наследник переопределяет только нужные части.
    /// </summary>
    [Serializable]
    public abstract class AttackModifierAura : PassiveEffectBase, ICardContextConsumer
    {
        [SerializeField] protected TargetSelectionType TargetSelection;

        public virtual void OnAppliedFromCard(EffectContext context) { }

        public virtual int GetDamageBonus(CardStatsData sourceCard) => 0;

        public virtual float GetDamageMultiplier(CardStatsData sourceCard) => 1f;

        public virtual List<CardAndHealthEntityOwnerData> ModifyTargets(
            List<CardAndHealthEntityOwnerData> targets, EffectContext context) => targets;

        public bool ResolveAura(CardAndHealthEntityOwnerData owner, CardAndHealthEntityOwnerData caster, GameSessionModel model) =>
            TargetScopeResolver.Resolve(TargetSelection, owner, null, model).Contains(caster);

        public override void Register(CardAndHealthEntityOwnerData owner) { }

        public override void Unregister() { }
    }
}
