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
        public List<CardStatsData> _cardsInDeck = new List<CardStatsData>();
        public List<CardStatsData> _cardsInHand = new List<CardStatsData>();

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
        List<GameplayCard> _cards { get; set; }
    }
}