using System.Collections.Generic;
using Feature.Data;
using Feature.GameSessionData;

using UnityEngine;

namespace Feature.Card.Script
{
    public class DeckFillSystem
    {
        private GameData _gameData;
        private GameSessionModel _gameSessionModel;

        public DeckFillSystem(GameData gameData, GameSessionModel gameSessionModel)
        {
            _gameData = gameData;
            _gameSessionModel = gameSessionModel;
        }

        public void InitializeDecks()
        {
            InitializeSingleDeck(_gameSessionModel.PlayerHero);
            InitializeSingleDeck(_gameSessionModel.EnemyHero);
        }

        private void InitializeSingleDeck(GameSessionPlayerData hero)
        {
            var allCards = _gameData.allCards;
            var heroDeck = hero.CardsInDeck;
            int cardsToAdd = hero.startCardsInDeckCount;
    
            heroDeck.CurrentValue.Clear();
    
            List<CardStatsData> availableCards = new List<CardStatsData>(allCards);
    
            for (int i = 0; i < cardsToAdd && availableCards.Count > 0; i++)
            {
                int randomIndex = Random.Range(0, availableCards.Count);
                heroDeck.CurrentValue.Add(availableCards[randomIndex]);
                availableCards.RemoveAt(randomIndex);
            }
    
            while (heroDeck.CurrentValue.Count < cardsToAdd)
            {
                heroDeck.CurrentValue.Add(allCards[Random.Range(0, allCards.Count)]);
            }
    
            Debug.Log($"Колода для {hero._heroName} инициализирована: {heroDeck.CurrentValue.Count} карт");
        }
    }
}