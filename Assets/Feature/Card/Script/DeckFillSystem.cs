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
        
        public void InitializeDeck(CardAndHealthEntityOwnerData hero)
        {

            hero.ClearDeck();

            List<CardStatsData> shuffled = new List<CardStatsData>(_gameData.allCards);

            for (int i = shuffled.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
            }

            for (int i = 0; i < hero.startCardsInDeckCount; i++)
            {
                var originalCard = shuffled[i % shuffled.Count];
        
                var cardCopy = ScriptableObject.Instantiate(originalCard);
                cardCopy.id = System.Guid.NewGuid().ToString();

                hero.AddCardToDeck(cardCopy);
            }

            Debug.Log($"Колода для {hero._heroName} инициализирована: {hero.CardsInDeck.CurrentValue.Count} карт");
        }
    }
}