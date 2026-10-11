using System.Collections.Generic;
using System.Linq;
using Feature.Card.Script;
using Feature.Data;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using Feature.Hero;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    /// <summary>
    /// Единственное место, где выбирается карта — и из конфигов, и из руки.
    /// CardPickFilter хранит только настройки, все проверки живут здесь.
    /// </summary>
    public class CardPoolPickSystem
    {
        private readonly GameData _gameData;

        public CardPoolPickSystem(GameData gameData) => _gameData = gameData;

        public CardStatsData Pick(CardPickFilter filter, EffectContext ctx) =>
            PickFrom(GetConfigCandidates(filter, ctx), filter, ctx);

        public CardStatsData PickFromHand(CardAndHealthEntityOwnerData handOwner, CardPickFilter filter, EffectContext ctx) =>
            PickFrom(handOwner.CardsInHand.CurrentValue, filter, ctx);

        #region Подбор

        /// <summary>
        /// Общий путь для любого источника: заданная в фильтре карта, иначе случайная из подходящих.
        /// </summary>
        private CardStatsData PickFrom(IEnumerable<CardStatsData> candidates, CardPickFilter filter, EffectContext ctx)
        {
            if (filter.SpecificCard != null) return filter.SpecificCard;

            var requiredClass = ResolveClass(filter, ctx);

            var pool = candidates
                .Where(card => card != null && MatchesType(card, filter) && MatchesClass(card, requiredClass))
                .ToList();

            return pool.Count == 0 ? null : pool[Random.Range(0, pool.Count)];
        }

        /// <summary>
        /// Кандидаты из конфигов: список класса кастующего, урезанный по допустимым редкостям.
        /// </summary>
        private IEnumerable<CardStatsData> GetConfigCandidates(CardPickFilter filter, EffectContext ctx)
        {
            var heroClass = ResolveClass(filter, ctx);
            var allowedRarities = ResolveRarities(filter, ctx, heroClass);

            return _gameData.GetCardsByClass(heroClass)
                .Where(card => allowedRarities.Contains(card.Rarity));
        }

        #endregion

        #region Проверки

        private AllHeroClass ResolveClass(CardPickFilter filter, EffectContext ctx)
        {
            if (filter.ClassSource == ClassSource.Manual)
                return filter.ManualClass;

            return ctx.GameSessionModel.GetPlayerDataByOwner(ctx.Caster)?.HeroClassData.MainClass
                   ?? AllHeroClass.All;
        }

        private bool MatchesType(CardStatsData card, CardPickFilter filter) => filter.CardType switch
        {
            CardTypeFilter.SpellOnly  => card is SpellCardData,
            CardTypeFilter.MinionOnly => card is MinionCardData,
            _                         => true
        };

        private bool MatchesClass(CardStatsData card, AllHeroClass requiredClass)
        {
            if (requiredClass == AllHeroClass.All) return true;

            return card.Specialization != null && card.Specialization.Contains(requiredClass);
        }

        private HashSet<CardRarity> ResolveRarities(CardPickFilter filter, EffectContext ctx, AllHeroClass heroClass)
        {
            if (filter.RaritySource == RaritySource.Any)
                return new HashSet<CardRarity> { CardRarity.Common, CardRarity.Hidden, CardRarity.Anomalous, CardRarity.Primordial };

            if (filter.RaritySource == RaritySource.Manual)
                return new HashSet<CardRarity>(filter.ManualRarities);

            var baseDeck = ctx.GameSessionModel.GetPlayerDataByOwner(ctx.Caster).MainHeroEntity().BaseDeck;

            var rarities = baseDeck
                .Where(c => c.Specialization.Contains(heroClass))
                .Select(c => c.Rarity)
                .ToHashSet();

            if (rarities.Count == 0)
                rarities.Add(CardRarity.Common);

            return rarities;
        }

        #endregion
    }
}
