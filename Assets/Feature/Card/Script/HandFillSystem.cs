using Feature.GameSessionData;
using System.Collections.Generic;
using Feature.Hero;
using UnityEngine;

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
        
        private void FillHandDataForHero(GameSessionPlayerData hero)
        {
            hero._cardsInHand.Clear();
            
            int cardsToDraw = hero.startCardsInHand;
            var deck = hero._cardsInDeck;
            
            Shuffle(deck);
            
            int cardsToTake = Mathf.Min(cardsToDraw, deck.Count);
            
            for (int i = 0; i < cardsToTake; i++)
            {
                CardStatsData drawnCard = deck[0];
                hero._cardsInHand.Add(drawnCard);
                deck.RemoveAt(0);
            }
            
            Debug.Log($"Рука {hero._heroName}: взято {hero._cardsInHand.Count} карт, в колоде осталось {deck.Count}");
            
        }
        
        private void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            }
        }
    }
}