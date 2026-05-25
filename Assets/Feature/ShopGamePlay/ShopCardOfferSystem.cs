using System.Collections.Generic;
using System.Linq;
using Feature.Card.Script;
using Feature.Data;
using UnityEngine;

namespace Feature.ShopGamePlay
{
    public class ShopCardOfferSystem
    {
        private readonly GameData _gameData;
        private const int OfferedCardsCount = 6;

        public ShopCardOfferSystem(GameData gameData)
        {
            _gameData = gameData;
        }

        public List<CardStatsData> GenerateCardOffers()
        {
            var allBaseLists = _gameData.GetAllBaseClassCards();
            var offers = new List<CardStatsData>();

            for (int i = 0; i < OfferedCardsCount; i++)
            {
                var randomClassList = allBaseLists[Random.Range(0, allBaseLists.Count)];

                var commonCards = randomClassList
                    .Where(c => c.Rarity == CardRarity.Common)
                    .ToList();

                if (commonCards.Count == 0) continue;

                var randomCard = commonCards[Random.Range(0, commonCards.Count)];
                offers.Add(randomCard);
            }

            return offers;
        }
    }
}