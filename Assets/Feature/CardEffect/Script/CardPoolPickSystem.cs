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
            return baseDeck
                .Where(c => c.Specialization.Contains(heroClass))
                .Select(c => c.Rarity)
                .ToHashSet();
        }

        private bool MatchesType(CardStatsData card, CardTypeFilter filter) => filter switch
        {
            CardTypeFilter.SpellOnly  => card is SpellCardData,
            CardTypeFilter.MinionOnly => card is MinionCardData,
            _                         => true
        };
    }
}
