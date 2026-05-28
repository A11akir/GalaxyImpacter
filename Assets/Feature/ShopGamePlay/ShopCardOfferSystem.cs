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
            var purchaseCount = _gameSessionModel.PlayerHero.HeroClassPurchaseCount;
            var offers = new List<CardStatsData>();

            for (int i = 0; i < OfferedCardsCount; i++)
            {
                // Взвешенный выбор класса вместо Random.Range
                int randomIndex = GetClassIndex(allBaseLists, purchaseCount);
                var randomClassList = allBaseLists[randomIndex];
                var randomClass = GetClassByIndex(randomIndex);

                int commonPurchases    = purchaseCount.GetPurchaseCount(randomClass, CardRarity.Common);
                int hiddenPurchases    = purchaseCount.GetPurchaseCount(randomClass, CardRarity.Hidden);
                int anomalousPurchases = purchaseCount.GetPurchaseCount(randomClass, CardRarity.Anomalous);
                int primordialPurchases = purchaseCount.GetPurchaseCount(randomClass, CardRarity.Primordial);

                var rarity = RollRarity(randomClass, commonPurchases, hiddenPurchases, anomalousPurchases, primordialPurchases);
                var card = PickCardByRarity(randomClassList, randomClass, rarity);

                if (card != null)
                    offers.Add(card);
            }

            return offers;
        }

        private CardRarity RollRarity(
            AllHeroClass heroClass,
            int commonPurchases,
            int hiddenPurchases,
            int anomalousPurchases,
            int primordialPurchases)
        {
            int wCommon     = 100;
            int wHidden     = CalculateRarityWeight(commonPurchases);
            int wAnomalous  = CalculateRarityWeight(hiddenPurchases);
            int wPrimordial = CalculateRarityWeight(anomalousPurchases + primordialPurchases);
            

            int total = wCommon + wHidden + wAnomalous + wPrimordial;
            int roll  = Random.Range(0, total);

            Debug.Log($"[ShopOffer] Class: {heroClass} | " +
                      $"Common: {wCommon}({100f * wCommon / total:F1}%) | " +
                      $"Hidden: {wHidden}({100f * wHidden / total:F1}%) | " +
                      $"Anomalous: {wAnomalous}({100f * wAnomalous / total:F1}%) | " +
                      $"Primordial: {wPrimordial}({100f * wPrimordial / total:F1}%) | " +
                      $"Roll: {roll}/{total}");

            if (roll < wCommon)                              return CardRarity.Common;
            if (roll < wCommon + wHidden)                    return CardRarity.Hidden;
            if (roll < wCommon + wHidden + wAnomalous)       return CardRarity.Anomalous;
            return CardRarity.Primordial;
        }

        private int GetClassIndex(List<List<CardStatsData>> allBaseLists, HeroClassPurchaseCount purchaseCount)
        {
            // Базовый вес каждого класса = 10
            // +1 за каждую купленную карту этого класса
            var weights = new int[allBaseLists.Count];
    
            for (int i = 0; i < allBaseLists.Count; i++)
            {
                var heroClass = GetClassByIndex(i);
                int totalPurchases = 
                    purchaseCount.GetPurchaseCount(heroClass, CardRarity.Common) +
                    purchaseCount.GetPurchaseCount(heroClass, CardRarity.Hidden) +
                    purchaseCount.GetPurchaseCount(heroClass, CardRarity.Anomalous) +
                    purchaseCount.GetPurchaseCount(heroClass, CardRarity.Primordial);
        
                weights[i] = 10 + totalPurchases;
            }

            // Взвешенный рандом
            int total = weights.Sum();
            int roll = Random.Range(0, total);
    
            int cumulative = 0;
            for (int i = 0; i < weights.Length; i++)
            {
                cumulative += weights[i];
                if (roll < cumulative)
                {
                    Debug.Log($"[ShopOffer] Class roll: {GetClassByIndex(i)} " +
                              $"weight={weights[i]}/{total} " +
                              $"({100f * weights[i] / total:F1}%)");
                    return i;
                }
            }

            return weights.Length - 1;
        }
        
        private CardStatsData PickCardByRarity(
            List<CardStatsData> classCards,
            AllHeroClass heroClass,
            CardRarity rarity)
        {
            var cards = classCards
                .Where(c => c.Rarity == rarity)
                .ToList();

            // Если карт нужной редкости нет — откатываемся на более низкую
            if (cards.Count == 0)
            {
                Debug.Log($"[ShopOffer] No {rarity} cards for {heroClass}, falling back...");

                if (rarity == CardRarity.Primordial)
                    return PickCardByRarity(classCards, heroClass, CardRarity.Anomalous);
                if (rarity == CardRarity.Anomalous)
                    return PickCardByRarity(classCards, heroClass, CardRarity.Hidden);
                if (rarity == CardRarity.Hidden)
                    return PickCardByRarity(classCards, heroClass, CardRarity.Common);

                Debug.LogWarning($"[ShopOffer] No cards at all for {heroClass}!");
                return null;
            }

            var card = cards[Random.Range(0, cards.Count)];
            Debug.Log($"[ShopOffer] → Selected {rarity} card: {card.Name}");
            return card;
        }

        private int CalculateRarityWeight(int purchaseCount)
        {
            if (purchaseCount <= 0) return 0;

            int weight = 0;
            for (int i = 0; i < purchaseCount && i < 10; i++)
                weight += 10 - i; // 10+9+8...+1 = макс 55

            return weight;
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