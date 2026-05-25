// BuyCardShopSystem.cs
using Feature.Card.Script;
using Feature.Data;
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
            AddClassFromCard(card);
        }

        private void AddCardToDeck(CardStatsData card)
        {
            var cardCopy = ScriptableObject.Instantiate(card);
            cardCopy.id = System.Guid.NewGuid().ToString();
            _gameSessionModel.PlayerHero.MainHeroEntity().AddCardToDeck(cardCopy);
        }

        private void AddClassFromCard(CardStatsData card)
        {
            var specs = card.Specialization;
            if (specs == null || specs.Count == 0) return;

            foreach (var spec in specs)
            {
                if (System.Enum.TryParse<AllHeroClass>(spec, out var heroClass))
                    _gameSessionModel.PlayerHero.HeroClassData.AddClass(heroClass);
            }
        }
    }
}