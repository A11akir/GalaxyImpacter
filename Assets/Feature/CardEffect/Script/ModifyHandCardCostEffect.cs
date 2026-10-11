using System;
using System.Linq;
using Feature.Card.Script;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using Feature.Hero;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    public enum CostChangeDirection
    {
        Reduce,   // 0 — значение, записанное в ассетах
        Increase
    }

    [Serializable]
    public class ModifyHandCardCostEffect : CardEffect
    {
        [SerializeField] private CardPickQuery _query = new();
        [SerializeField] private CostChangeDirection _direction;

        public override void Execute(EffectContext context)
        {
            var values = context.CardData.Values;
            if (values == null || context.ValueIndex >= values.Count) return;

            int amount = values[context.ValueIndex];
            if (amount == 0) return;

            foreach (var target in ResolveTargets(context))
            {
                if (target == null) continue;

                var card = PickCardInHand(target, context);
                if (card == null) continue;

                card.Cost = _direction == CostChangeDirection.Reduce
                    ? Mathf.Max(0, card.Cost - amount)
                    : card.Cost + amount;
            }
        }

        private CardStatsData PickCardInHand(CardAndHealthEntityOwnerData owner, EffectContext context)
        {
            if (_query.SpecificCard != null) return _query.SpecificCard;

            var hand = owner.CardsInHand.CurrentValue;
            if (hand == null || hand.Count == 0) return null;

            var pool = hand
                .Where(card => MatchesType(card) && MatchesClass(card, context))
                .ToList();

            return pool.Count == 0 ? null : pool[UnityEngine.Random.Range(0, pool.Count)];
        }

        private bool MatchesType(CardStatsData card) => _query.CardType switch
        {
            CardTypeFilter.SpellOnly  => card is SpellCardData,
            CardTypeFilter.MinionOnly => card is MinionCardData,
            _                         => true
        };

        private bool MatchesClass(CardStatsData card, EffectContext context)
        {
            var heroClass = _query.ClassSource == ClassSource.Manual
                ? _query.ManualClass
                : context.GameSessionModel.GetPlayerDataByOwner(context.Caster)?.HeroClassData.MainClass
                  ?? AllHeroClass.All;

            if (heroClass == AllHeroClass.All) return true;

            return card.Specialization != null && card.Specialization.Contains(heroClass);
        }
    }
}
