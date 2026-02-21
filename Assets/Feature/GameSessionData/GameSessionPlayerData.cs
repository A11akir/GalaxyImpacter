using System;
using System.Collections.Generic;
using Feature.Card.Script;
using Feature.Hero;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Feature.GameSessionData
{
    public class GameSessionPlayerData 
    {
        private readonly ReactiveProperty<List<CardStatsData>> _cardsInDeck = new(new List<CardStatsData>());
        public ReadOnlyReactiveProperty<List<CardStatsData>> CardsInDeck => _cardsInDeck;

        public int LastRemovedCardIndex;
        public List<CardStatsData> CardsInDeckList
        {
            get => _cardsInDeck.Value;
            set => _cardsInDeck.Value = value;
        }

        public readonly ReactiveProperty<List<CardStatsData>> _cardsInHand = new(new List<CardStatsData>());
        public ReadOnlyReactiveProperty<List<CardStatsData>> CardsInHand => _cardsInHand;
        
        public List<CardStatsData> CardsInHandList
        {
            get => _cardsInHand.Value;
            set => _cardsInHand.Value = value;
        }
        
        private readonly ReactiveProperty<List<CardStatsData>> _cardsInBoard = new(new List<CardStatsData>());
        public ReadOnlyReactiveProperty<List<CardStatsData>> CardsInBoard => _cardsInBoard;
        
        public List<CardStatsData> CardsInBoardList
        {
            get => _cardsInBoard.Value;
            set => _cardsInBoard.Value = value;
        }

        public int CardsInBoardMax = 5;
        public int maxCardsInHandCount = 10;
        public int startCardsInHand = 4;
        public int startCardsInDeckCount = 6;

        public bool IsPlayerFirst;

        public string _heroName;

        public int _heroPowerCost;
        public int _health;
        private readonly ReactiveProperty<int> _currencyCount = new();
        public ReadOnlyReactiveProperty<int> CurrencyCount => _currencyCount;
        
        private readonly ReactiveProperty<int> _chakraCount = new();
        public ReadOnlyReactiveProperty<int> ChakraCount => _chakraCount;

        public int MaxChakraCountBaseIncrease = 8;

        public int Currency
        {
            get => _currencyCount.Value;
            set => _currencyCount.Value = value;
        }
        public int Chakra
        {
            get => _chakraCount.Value;
            set => _chakraCount.Value = value;
        }
        public Sprite _iconImage;
        public Sprite _heroPowerSprite;

        public bool PlayerHasHero()
        {
            if (_heroName != null && _heroPowerCost != null && _health != null)
            {
                return true;
            }
            return false;
        }
        
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
        
        public CardStatsData DrawCardFromDeck()
        {
            if (_cardsInDeck.Value.Count == 0) return null;
            
            var currentList = new List<CardStatsData>(_cardsInDeck.Value);
            var drawnCard = currentList[0];
            currentList.RemoveAt(0);
            _cardsInDeck.Value = currentList;
            
            return drawnCard;
        }
        
        public void ShuffleDeck()
        {
            var currentList = new List<CardStatsData>(_cardsInDeck.Value);
            for (int i = currentList.Count - 1; i > 0; i--)
            {
                int randomIndex = UnityEngine.Random.Range(0, i + 1);
                (currentList[i], currentList[randomIndex]) = (currentList[randomIndex], currentList[i]);
            }
            _cardsInDeck.Value = currentList;
        }
        
        public void AddCardToHand(CardStatsData card)
        {
            if (_cardsInHand.Value.Count >= maxCardsInHandCount) return;
            
            var currentList = new List<CardStatsData>(_cardsInHand.Value);
            currentList.Add(card);
            _cardsInHand.Value = currentList;
        }
        
        public void RemoveCardFromHand(HandCardData card)
        {
            card.Index = LastRemovedCardIndex;
            var currentList = new List<CardStatsData>(_cardsInHand.Value);
            currentList.Remove(card.Data);
            _cardsInHand.Value = currentList;
            
        }
        
        public void ClearHand()
        {
            _cardsInHand.Value = new List<CardStatsData>();
        }
        
        public void AddCardToBoard(CardStatsData card)
        {
            if (_cardsInBoard.Value.Count >= CardsInBoardMax) return;
            
            var currentList = new List<CardStatsData>(_cardsInBoard.Value);
            currentList.Add(card);
            _cardsInBoard.Value = currentList;
        }
        
        public void RemoveCardFromBoard(CardStatsData card)
        {
            var currentList = new List<CardStatsData>(_cardsInBoard.Value);
            currentList.Remove(card);
            _cardsInBoard.Value = currentList;
        }
        
        public void ClearBoard()
        {
            _cardsInBoard.Value = new List<CardStatsData>();
        }
        
        public void SetChakra(int amount)
        {
            _chakraCount.Value = amount;
        }
    }
    
    public class Health
    {
        public event Action HpChanged;
        public event Action HealthOver;

        private float _currentHp;
        public float CurrentHp
        {
            get => _currentHp;
            private set
            {
                if (!Mathf.Approximately(_currentHp, value))
                {
                    _currentHp = value;
                    HpChanged?.Invoke();
                }
            }
        }

        public float MaxHp { get; set; }
        public void ResetHp() => CurrentHp = MaxHp;
        public void Heal(float amount)
        {
            
        }

        public void TakeDamage(float damage)
        {
            CurrentHp -= damage;

            if (CurrentHp <= 0)
            {
                HealthOver?.Invoke();
            }
        }
    }

    public class HeroPowerData
    {
        private int cost;
        
    }

    public interface ITargetable
    {
        
    }
    public interface IUnTargetable
    {
        
    }

    public interface IMinion
    {
        List<GameplayLogicCard> _cards { get; set; }
    }
}