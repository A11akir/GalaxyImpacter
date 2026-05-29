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
            var allowedRarities = ResolveRarities(query, ctx, heroClass);

            var pool = _gameData.GetCardsByClass(heroClass)
                .Where(c => MatchesType(c, query.CardType) && allowedRarities.Contains(c.Rarity))
                .ToList();

            Debug.Log($"[CardPoolPickSystem] class={heroClass} rarities=[{string.Join(",", allowedRarities)}] type={query.CardType} pool={pool.Count}");

            if (pool.Count == 0) return null;
            return pool[Random.Range(0, pool.Count)];
        }

        private AllHeroClass ResolveClass(CardPickQuery query, EffectContext ctx)
        {
            if (query.ClassSource == ClassSource.Manual)
                return query.ManualClass;

            var playerData = ctx.GameSessionModel.GetPlayerDataByOwner(ctx.Caster);
            var heroClass = playerData.HeroClassData.MainClass;
            Debug.Log($"[CardPoolPickSystem] ResolveClass: caster={ctx.Caster._heroName} playerData={playerData != null} mainClass={heroClass}");
            return heroClass;
        }

        private HashSet<CardRarity> ResolveRarities(CardPickQuery query, EffectContext ctx, AllHeroClass heroClass)
        {
            if (query.RaritySource == RaritySource.Any)
                return new HashSet<CardRarity> { CardRarity.Common, CardRarity.Hidden, CardRarity.Anomalous, CardRarity.Primordial };

            if (query.RaritySource == RaritySource.Manual)
                return new HashSet<CardRarity>(query.ManualRarities);

            var classStr = heroClass.ToString();
            var baseDeck = ctx.GameSessionModel.GetPlayerDataByOwner(ctx.Caster).MainHeroEntity().BaseDeck;
            var rarities = baseDeck
                .Where(c => c.Specialization.Contains(classStr))
                .Select(c => c.Rarity)
                .ToHashSet();
            Debug.Log($"[CardPoolPickSystem] ResolveRarities: baseDeck={baseDeck.Count} classStr={classStr} matchingCards={baseDeck.Count(c => c.Specialization.Contains(classStr))} rarities=[{string.Join(",", rarities)}]");
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
