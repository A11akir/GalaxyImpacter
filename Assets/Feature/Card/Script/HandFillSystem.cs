using Feature.GameSessionData;
using System.Collections.Generic;
using Feature.Hero;
using UnityEngine;
using R3;

namespace Feature.Card.Script
{
    public class HandFillSystem
    {
        private GameSessionModel _gameSessionModel;
        
        public HandFillSystem(GameSessionModel gameSessionModel) => _gameSessionModel = gameSessionModel;

        public void FillHandDataInDecks()
        {
            FillHandDataForHero(_gameSessionModel.PlayerHero);
            FillHandDataForHero(_gameSessionModel.EnemyHero);
        }
        
        public List<CardStatsData> GetHandData()
        {
            return _gameSessionModel.PlayerHero.CardsInHand.CurrentValue;
        }
        
        
        private void FillHandDataForHero(GameSessionPlayerData hero)
        {
            hero.ClearHand();
            
            int cardsToDraw = hero.startCardsInHand;
            
            hero.ShuffleDeck();
            

            for (int i = 0; i < cardsToDraw; i++)
            {
                CardStatsData drawnCard = hero.DrawCardFromDeck();
                if (drawnCard != null)
                {
                    hero.AddCardToHand(drawnCard);
                }
                else
                {
                    Debug.Log($"Колода {hero._heroName} пуста");
                    break;
                }
            }
            
            Debug.Log($"Рука {hero._heroName}: взято {hero.CardsInHand.CurrentValue.Count} карт, в колоде осталось {hero.CardsInDeck.CurrentValue.Count}");
        }
        
        
    }
}