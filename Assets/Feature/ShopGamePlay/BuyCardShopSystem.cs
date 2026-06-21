using System.Collections.Generic;
using Feature.GameSessionData;
using Feature.Hero;
using UnityEngine;

namespace Feature.ShopGamePlay
{
    public class BuyCardShopSystem
    {
        private readonly GameSessionModel _gameSessionModel;

        public BuyCardShopSystem(GameSessionModel gameSessionModel)
        {
            _gameSessionModel = gameSessionModel;
        }

        public void BuyCard(CardStatsData card)
        {
            AddCardToDeck(card);
        }

        private void AddCardToDeck(CardStatsData card)
        {
            var cardCopy = ScriptableObject.Instantiate(card);
            cardCopy.id = System.Guid.NewGuid().ToString();
    
            var hero = _gameSessionModel.PlayerHero.MainHeroEntity();
    
            var updatedBaseDeck = new List<CardStatsData>(hero.BaseDeck) { cardCopy };
            hero.SetBaseDeck(updatedBaseDeck);
    
            hero.AddCardToDeck(cardCopy);
            AddClassFromCard(card);
        }
    
        private void AddClassFromCard(CardStatsData card)
        {
            var specs = card.Specialization;
            if (specs == null || specs.Count == 0) return;

            foreach (var heroClass in specs)
            {
                if (heroClass == AllHeroClass.All) continue;

                _gameSessionModel.PlayerHero.HeroClassData.AddClass(heroClass);
                _gameSessionModel.PlayerHero.HeroClassPurchaseCount
                    .AddPurchase(heroClass, card.Rarity);
            }
        }
    }
}