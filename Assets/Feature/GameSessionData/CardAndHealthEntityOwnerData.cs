using System.Collections.Generic;
using System.Linq;
using Feature.Card.Script;
using Feature.Common;
using Feature.GoogleSheets;
using R3;
using UnityEngine;

namespace Feature.GameSessionData
{
    public class CardAndHealthEntityOwnerData
    {
        public int CountCardsInHand => _cardsInHand.Value.Count;
        private readonly int maxCardsInHandCount = 10;
        public int startCardsInHandToDraw = 4;
        public int startCardsInDeckCount = 10;
        
        public List<SpellCardData> SpellsList;
        
        private readonly ReactiveProperty<List<CardStatsData>> _cardsInDeck = new(new List<CardStatsData>());
        public ReadOnlyReactiveProperty<List<CardStatsData>> CardsInDeck => _cardsInDeck;

        private readonly ReactiveProperty<List<CardStatsData>> _cardsInHand = new(new List<CardStatsData>(6));
        public ReadOnlyReactiveProperty<List<CardStatsData>> CardsInHand => _cardsInHand;
        
        private List<CardStatsData> _baseDeck = new();
        
        public List<CardStatsData> CardsInHandList
        {
            get => _cardsInHand.Value;
            set => _cardsInHand.Value = value;
        }

        public int StartChakra = 2;
        
        private readonly ReactiveProperty<List<CardStatsData>> _cardsInBoard = new(new List<CardStatsData>());
        public ReadOnlyReactiveProperty<List<CardStatsData>> CardsInBoard => _cardsInBoard;
        
        public List<CardStatsData> CardsInBoardList
        {
            get => _cardsInBoard.Value;
            set => _cardsInBoard.Value = value;
        }
        
        private readonly ReactiveProperty<int> _health = new();
        public ReadOnlyReactiveProperty<int> Health => _health;

        public int HealthValue
        {
            get => _health.Value;
            set => _health.Value = value;
        }
        
        public string _heroName;
        
        private readonly ReactiveProperty<int> _chakraCount = new();
        public ReadOnlyReactiveProperty<int> ChakraCount => _chakraCount;

        public int MaxChakraCountBaseIncrease = 8;
        
        public int Chakra
        {
            get => _chakraCount.Value;
            set => _chakraCount.Value = value;
        }

        public string CardId { get; set; }

        public Sprite _iconImage;
        
        public void AddCardToDeck(CardStatsData card)
        {
            var currentList = new List<CardStatsData>(_cardsInDeck.Value);
            currentList.Add(card);
            _cardsInDeck.Value = currentList;
        }
        
        public void RemoveCardFromDeck(CardStatsData card)
        {
            var currentList = new List<CardStatsData>(_cardsInDeck.Value);
            currentList.Remove(card);
            _cardsInDeck.Value = currentList;
        }
        
        public void ClearDeck()
        {
            _cardsInDeck.Value = new List<CardStatsData>();
        }
        
        public void SetBaseDeck(List<CardStatsData> cards)
        {
            _baseDeck = new List<CardStatsData>(cards);
        }
        
        private void RefillDeckFromBase()
        {
            var refilled = _baseDeck.Select(card => 
            {
                var copy = ScriptableObject.Instantiate(card);
                copy.id = System.Guid.NewGuid().ToString();
                return copy;
            }).ToList();
    
            for (int i = refilled.Count - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                (refilled[i], refilled[j]) = (refilled[j], refilled[i]);
            }
            _cardsInDeck.Value = refilled;
        
        }
        
        public void DrawCardFromDeck()
        {
    
            if (_cardsInDeck.Value.Count == 0)
            {
                RefillDeckFromBase();
            }
    
            if (_cardsInDeck.Value.Count == 0)
            {
                return;
            }

            var deckList = new List<CardStatsData>(_cardsInDeck.Value);
            var drawnCard = deckList[0];
            deckList.RemoveAt(0);
            _cardsInDeck.Value = deckList;


            AddCardToHand(drawnCard, CountCardsInHand);
        }

        public void DiscardHand()
        {
            _cardsInHand.Value = new List<CardStatsData>();
        }
        
        public void ShuffleDeck()
        {
            var currentList = new List<CardStatsData>(_cardsInDeck.Value);
            for (int i = currentList.Count - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                (currentList[i], currentList[randomIndex]) = (currentList[randomIndex], currentList[i]);
            }
            _cardsInDeck.Value = currentList;
        }

        private void AddCardToHand(CardStatsData card, int index)
        {
            if (_cardsInHand.Value.Count >= maxCardsInHandCount) return;
    
            var newList = new List<CardStatsData>(_cardsInHand.Value);
    
            newList.Insert(index, card);
    
            _cardsInHand.Value = newList;
    
        }
        
        public void RemoveCardFromHand(CardStatsData data)
        {
            var newList = new List<CardStatsData>(_cardsInHand.Value);
    
            newList.Remove(data);
    
            _cardsInHand.Value = newList;
        }
        
        public void SetChakra(int amount)
        {
            _chakraCount.Value = amount;
        }
    }
}