using System.Collections.Generic;
using System.Linq;
using Feature.Card.Script;
using Feature.Data;
using Feature.GoogleSheets;
using Feature.Hero;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    public class CardPoolPickSystem
    {
        private readonly GameData _gameData;

        public CardPoolPickSystem(GameData gameData) => _gameData = gameData;

        public CardStatsData Pick(CardPickQuery query, EffectContext ctx)
        {
            var heroClass = ResolveClass(query, ctx);
            Debug.Log($"[CardPoolPickSystem] heroClass={heroClass}");

            var allowedRarities = ResolveRarities(query, ctx, heroClass);
            Debug.Log($"[CardPoolPickSystem] allowedRarities=[{string.Join(",", allowedRarities)}]");

            var allCardsOfClass = _gameData.GetCardsByClass(heroClass);
            Debug.Log($"[CardPoolPickSystem] cards in class={allCardsOfClass.Count}");

            var pool = allCardsOfClass
                .Where(c => MatchesType(c, query.CardType) && allowedRarities.Contains(c.Rarity))
                .ToList();
            Debug.Log($"[CardPoolPickSystem] pool after filter={pool.Count}");

            if (pool.Count == 0) return null;
            return pool[Random.Range(0, pool.Count)];
        }

        private AllHeroClass ResolveClass(CardPickQuery query, EffectContext ctx)
        {
            if (query.ClassSource == ClassSource.Manual)
                return query.ManualClass;

            var playerData = ctx.GameSessionModel.GetPlayerDataByOwner(ctx.Caster);
            var heroClass = playerData.HeroClassData.MainClass;
            return heroClass;
        }

        private HashSet<CardRarity> ResolveRarities(CardPickQuery query, EffectContext ctx, AllHeroClass heroClass)
        {
            if (query.RaritySource == RaritySource.Any)
                return new HashSet<CardRarity> { CardRarity.Common, CardRarity.Hidden, CardRarity.Anomalous, CardRarity.Primordial };

            if (query.RaritySource == RaritySource.Manual)
                return new HashSet<CardRarity>(query.ManualRarities);

            var baseDeck = ctx.GameSessionModel.GetPlayerDataByOwner(ctx.Caster).MainHeroEntity().BaseDeck;

            var rarities = baseDeck
                .Where(c => c.Specialization.Contains(heroClass))
                .Select(c => c.Rarity)
                .ToHashSet();

            if (rarities.Count == 0)
                rarities.Add(CardRarity.Common); // ← fallback, если в колоде нет карт этого класса

            return rarities;
        }

        private bool MatchesType(CardStatsData card, CardTypeFilter filter) => filter switch
        {
            CardTypeFilter.SpellOnly  => card is SpellCardData,
            CardTypeFilter.MinionOnly => card is MinionCardData,
            _                         => true
        };
    }
}
