using System;
using System.Collections.Generic;
using System.Linq;
using Feature.CardEffect.Script;
using Feature.GameSessionData;
using R3;
using UnityEngine;

namespace Feature.PassiveEffect.Script
{
    /// <summary>
    /// Аура модификатора урона: бонус получают те, кто попадает в TargetSelection
    /// относительно владельца ауры.
    /// </summary>
    [Serializable]
    public class DamageBonusAura : PassiveEffectBase, ICardContextConsumer, IValueProvider, IStackablePassive
    {
        [SerializeField] protected TargetSelectionType TargetSelection = TargetSelectionType.Allies;

        private readonly ReactiveProperty<int> _bonus = new(0);
        public ReadOnlyReactiveProperty<int> Value => _bonus;

        protected CardAndHealthEntityOwnerData Owner;

        public void OnAppliedFromCard(EffectContext context) =>
            AddBonus(context.CardData.Values[context.ValueIndex]);

        public void AddBonus(int amount) => _bonus.Value += amount;

        // Владельца берём отсюда: Register вызывается lifecycle'ом при каждом добавлении.
        public override void Register(CardAndHealthEntityOwnerData owner)
        {
            Owner = owner;
        }

        public override void Unregister() { }

        public virtual int GetDamageBonus(CardStatsData sourceCard) => _bonus.Value;

        public bool Covers(CardAndHealthEntityOwnerData caster)
        {
            if (caster == null || Owner == null) return false;

            var model = Owner.GameSessionModel;
            if (model == null) return false;

            return ResolveTargets(model).Contains(caster);
        }

        private List<CardAndHealthEntityOwnerData> ResolveTargets(GameSessionModel model)
        {
            var side = model.GetPlayerDataByOwner(Owner);
            bool ownerIsPlayer = side == model.PlayerHero;
            var ally  = ownerIsPlayer ? model.PlayerHero : model.EnemyHero;
            var enemy = ownerIsPlayer ? model.EnemyHero : model.PlayerHero;

            return TargetSelection switch
            {
                TargetSelectionType.Self         => new List<CardAndHealthEntityOwnerData> { Owner },
                TargetSelectionType.PlayerHero   => new List<CardAndHealthEntityOwnerData> { model.PlayerHero.MainHeroEntity() },
                TargetSelectionType.EnemyHero    => new List<CardAndHealthEntityOwnerData> { model.EnemyHero.MainHeroEntity() },
                TargetSelectionType.EnemyMinion  => enemy.Minions.ToList(),
                TargetSelectionType.PlayerMinion => ally.Minions.ToList(),
                TargetSelectionType.Allies       => ally.CardAndHealthEntityOwners.ToList(),
                TargetSelectionType.Enemies      => enemy.CardAndHealthEntityOwners.ToList(),
                TargetSelectionType.All          => model.GetAllEntityOwners().ToList(),
                _                                => new List<CardAndHealthEntityOwnerData>()
            };
        }

        public override PassiveEffectBase Clone() =>
            new DamageBonusAura { Config = Config, Duration = Duration, TargetSelection = TargetSelection };
    }
}
