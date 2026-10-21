using System;
using Feature.CardEffect.Script;
using Feature.GameSessionData;
using R3;
using UnityEngine;

namespace Feature.PassiveEffect.Script
{
    [Serializable]
    public class DamageBonusAura : PassiveEffectBase, ICardContextConsumer, IValueProvider, IStackablePassive
    {
        [SerializeField] protected TargetSelectionType TargetSelection = TargetSelectionType.Allies;

        private readonly ReactiveProperty<int> _bonus = new(0);
        public ReadOnlyReactiveProperty<int> Value => _bonus;

        public void OnAppliedFromCard(EffectContext context) =>
            AddBonus(context.CardData.Values[context.ValueIndex]);

        public void AddBonus(int amount) => _bonus.Value += amount;

        public override void Register(CardAndHealthEntityOwnerData owner) { }

        public override void Unregister() { }

        public virtual int GetDamageBonus(CardStatsData sourceCard) => _bonus.Value;

        public virtual float GetDamageMultiplier(CardStatsData sourceCard) => 1f;

        public bool ResolveAura(CardAndHealthEntityOwnerData owner, CardAndHealthEntityOwnerData caster, GameSessionModel model) =>
            TargetScopeResolver.Resolve(TargetSelection, owner, null, model).Contains(caster);

        public override PassiveEffectBase Clone() =>
            new DamageBonusAura { Config = Config, Duration = Duration, TargetSelection = TargetSelection };
    }
}
