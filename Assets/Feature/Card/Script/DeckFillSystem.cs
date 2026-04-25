using System.Collections.Generic;
using System.Linq;
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
    
            List<CardStatsData> source;

            if (hero.SpellsList != null && hero.SpellsList.Count > 0)
                source = new List<CardStatsData>(hero.SpellsList);
            else
                source = new List<CardStatsData>(_gameData.allCards);
            
            for (int i = source.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (source[i], source[j]) = (source[j], source[i]);
            }


            int count = Mathf.Min(hero.startCardsInDeckCount, source.Count);
            for (int i = 0; i < count; i++)
            {
                var cardCopy = ScriptableObject.Instantiate(source[i]);
                cardCopy.id = System.Guid.NewGuid().ToString();
                hero.AddCardToDeck(cardCopy);
            }
    
            hero.SetBaseDeck(hero.CardsInDeck.CurrentValue.ToList());
        }
    }
}