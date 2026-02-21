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
            Debug.Log("Initializing decks");
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
                var originalCard = availableCards[randomIndex];
        
                var cardCopy = ScriptableObject.Instantiate(originalCard);
                cardCopy.id = System.Guid.NewGuid().ToString();
        
                heroDeck.CurrentValue.Add(cardCopy);
                availableCards.RemoveAt(randomIndex);
            }

            while (heroDeck.CurrentValue.Count < cardsToAdd)
            {
                var originalCard = allCards[Random.Range(0, allCards.Count)];
                var cardCopy = ScriptableObject.Instantiate(originalCard);
                cardCopy.id = System.Guid.NewGuid().ToString();
        
                heroDeck.CurrentValue.Add(cardCopy);
            }

            Debug.Log($"Колода для {hero._heroName} инициализирована: {heroDeck.CurrentValue.Count} карт");
        }
    }
}