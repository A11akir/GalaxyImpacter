using System.Collections.Generic;
using System.Linq;
using Feature.Card.Script;
using Feature.Data;
using Feature.GameSessionData;
using Feature.Hero;
using UnityEngine;

namespace Feature.ShopGamePlay
{
    public class ShopCardOfferSystem
    {
        private readonly GameData _gameData;
        private readonly GameSessionModel _gameSessionModel;
        private const int OfferedCardsCount = 6;

        public ShopCardOfferSystem(GameData gameData, GameSessionModel gameSessionModel)
        {
            _gameData = gameData;
            _gameSessionModel = gameSessionModel;
        }

        public List<CardStatsData> GenerateCardOffers()
        {
            var allBaseLists = _gameData.GetAllBaseClassCards();
            var heroClassData = _gameSessionModel.PlayerHero.HeroClassData;
            var offers = new List<CardStatsData>();

            for (int i = 0; i < OfferedCardsCount; i++)
            {
                int randomIndex = Random.Range(0, allBaseLists.Count);
                var randomClassList = allBaseLists[randomIndex];
                var randomClass = GetClassByIndex(randomIndex);
                
                var availableCards = GetAvailableCards(randomClassList, randomClass, heroClassData);

                if (availableCards.Count == 0) continue;

                offers.Add(availableCards[Random.Range(0, availableCards.Count)]);
            }

            return offers;
        }

        private List<CardStatsData> GetAvailableCards(
            List<CardStatsData> classCards,
            AllHeroClass heroClass,
            HeroClassData heroClassData)
        {
            bool hasClass = heroClassData.HasClass(heroClass);

            if (hasClass)
                return classCards
                    .Where(c => c.Rarity == CardRarity.Common || c.Rarity == CardRarity.Hidden)
                    .ToList();

            return classCards
                .Where(c => c.Rarity == CardRarity.Common)
                .ToList();
        }

        private AllHeroClass GetClassByIndex(int index)
        {
            return index switch
            {
                0 => AllHeroClass.Alchemist,
                1 => AllHeroClass.Assassin,
                2 => AllHeroClass.EarthMage,
                3 => AllHeroClass.Explorer,
                4 => AllHeroClass.FireMage,
                5 => AllHeroClass.Monster,
                6 => AllHeroClass.Warrior,
                7 => AllHeroClass.WaterMage,
                8 => AllHeroClass.WindMage,
                _ => AllHeroClass.All
            };
        }
    }
}