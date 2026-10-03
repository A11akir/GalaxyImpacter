using System;
using System.Linq;
using Feature.Card.Script;
using Feature.GoogleSheets;
using Feature.Hero;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    public enum CostModifyDirection
    {
        Decrease,
        Increase
    }

    [Serializable]
    public class ModifyHandCardCostEffect : CardEffect
    {
        [SerializeField] private CardPickQuery _query;
        [SerializeField] private CostModifyDirection _direction = CostModifyDirection.Decrease;

        public override void Execute(EffectContext ctx)
        {
            int amount = ctx.CardData.Values[ctx.ValueIndex];
            var targets = ResolveTargets(ctx);

            Debug.Log($"[ModifyHandCardCostEffect] amount={amount}, targets.Count={targets.Count}");

            foreach (var target in targets)
            {
                Debug.Log($"[ModifyHandCardCostEffect] target={target._heroName}, CardsInHand.Count={target.CardsInHandList.Count}");

                var candidates = target.CardsInHandList
                    .Where(c => MatchesQuery(c, _query, ctx))
                    .ToList();

                Debug.Log($"[ModifyHandCardCostEffect] candidates.Count={candidates.Count}");

                if (candidates.Count == 0) continue;

                var picked = candidates[UnityEngine.Random.Range(0, candidates.Count)];
                int delta = _direction == CostModifyDirection.Decrease ? -amount : amount;

                Debug.Log($"[ModifyHandCardCostEffect] picked={picked.Name}, delta={delta}, costBefore={picked.Cost}");

                ctx.HandCostModifierSystem.ApplyTemporaryModifier(target, picked, delta);

                Debug.Log($"[ModifyHandCardCostEffect] costAfter={picked.Cost}");
            }
        }

        private bool MatchesQuery(CardStatsData card, CardPickQuery query, EffectContext ctx)
        {
            bool classMatches = query.ClassSource == ClassSource.Manual
                ? card.Specialization.Contains(query.ManualClass)
                : card.Specialization.Contains(GetCasterClass(ctx));

            bool typeMatches = query.CardType switch
            {
                CardTypeFilter.SpellOnly => card is SpellCardData,
                CardTypeFilter.MinionOnly => card is MinionCardData,
                _ => true
            };

            Debug.Log($"[MatchesQuery] card={card.Name}, classMatches={classMatches}, typeMatches={typeMatches}");

            return classMatches && typeMatches;
        }

        private AllHeroClass GetCasterClass(EffectContext ctx)
        {
            var playerData = ctx.GameSessionModel.GetPlayerDataByOwner(ctx.Caster);
            return playerData.HeroClassData.MainClass;
        }
    }
}