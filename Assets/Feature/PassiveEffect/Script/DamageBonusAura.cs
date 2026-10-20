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
        protected GameSessionModel GameSessionModel;

        public void OnAppliedFromCard(EffectContext context)
        {
            GameSessionModel = context.GameSessionModel;
            AddBonus(context.CardData.Values[context.ValueIndex]);
        }

        public void AddBonus(int amount) => _bonus.Value += amount;

        // Владельца берём отсюда: Register вызывается lifecycle'ом при каждом добавлении,
        // и owner тут всегда корректен (в отличие от context.Target в OnAppliedFromCard).
        public override void Register(CardAndHealthEntityOwnerData owner)
        {
            Owner = owner;
        }

        public override void Unregister() { }

        public virtual int GetDamageBonus(CardStatsData sourceCard) => _bonus.Value;

        /// <summary>
        /// Попадает ли кастер в зону действия ауры.
        /// </summary>
        public bool Covers(CardAndHealthEntityOwnerData caster)
        {
            if (caster == null || Owner == null)
                return false;

            // Self не требует модели: просто совпадение с владельцем.
            if (TargetSelection == TargetSelectionType.Self)
                return Owner == caster;

            if (GameSessionModel == null)
                return false;

            return ResolveTargets().Contains(caster);
        }

        private List<CardAndHealthEntityOwnerData> ResolveTargets()
        {
            var side = GameSessionModel.GetPlayerDataByOwner(Owner);
            bool ownerIsPlayer = side == GameSessionModel.PlayerHero;
            var ally  = ownerIsPlayer ? GameSessionModel.PlayerHero : GameSessionModel.EnemyHero;
            var enemy = ownerIsPlayer ? GameSessionModel.EnemyHero : GameSessionModel.PlayerHero;

            return TargetSelection switch
            {
                TargetSelectionType.Self         => new List<CardAndHealthEntityOwnerData> { Owner },
                TargetSelectionType.PlayerHero   => new List<CardAndHealthEntityOwnerData> { GameSessionModel.PlayerHero.MainHeroEntity() },
                TargetSelectionType.EnemyHero    => new List<CardAndHealthEntityOwnerData> { GameSessionModel.EnemyHero.MainHeroEntity() },
                TargetSelectionType.EnemyMinion  => enemy.Minions.ToList(),
                TargetSelectionType.PlayerMinion => ally.Minions.ToList(),
                TargetSelectionType.Allies       => ally.CardAndHealthEntityOwners.ToList(),
                TargetSelectionType.Enemies      => enemy.CardAndHealthEntityOwners.ToList(),
                TargetSelectionType.All          => GameSessionModel.GetAllEntityOwners().ToList(),
                _                                => new List<CardAndHealthEntityOwnerData>()
            };
        }

        public override PassiveEffectBase Clone() =>
            new DamageBonusAura { Config = Config, Duration = Duration, TargetSelection = TargetSelection };
    }
}
