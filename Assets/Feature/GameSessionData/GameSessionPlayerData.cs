using System;
using System.Collections.Generic;
using Feature.Card.Script;
using Feature.GoogleSheets;
using R3;
using UnityEngine;

namespace Feature.GameSessionData
{
    public class GameSessionPlayerData 
    {
        public List<CardAndHealthEntityOwnerData> CardAndHealthEntityOwners = new List<CardAndHealthEntityOwnerData>{new CardAndHealthEntityOwnerData()};

        public CardAndHealthEntityOwnerData MainHeroEntity() => CardAndHealthEntityOwners[0];
        
        public int CardsInBoardMax = 6;

        private readonly ReactiveProperty<List<MinionCardData>> _cardsInBoard = new(new List<MinionCardData>());
        public ReadOnlyReactiveProperty<List<MinionCardData>> CardsInBoard => _cardsInBoard;
        
        public List<MinionCardData> CardsInBoardList
        {
            get => _cardsInBoard.Value;
            set => _cardsInBoard.Value = value;
        }
        
        public bool IsPlayerFirst;

        /*public string _heroName;*/

        public int _heroPowerCost;

        private readonly ReactiveProperty<int> _currencyCount = new();
        public ReadOnlyReactiveProperty<int> CurrencyCount => _currencyCount;
        
        public int Currency
        {
            get => _currencyCount.Value;
            set => _currencyCount.Value = value;
        }

        public Sprite _iconImage;
        public Sprite _heroPowerSprite;

        public bool PlayerHasHero()
        {
            return MainHeroEntity()._heroName != null && 
                   MainHeroEntity()._health > 0;
        }
        
        public void AddCardToBoard(MinionCardData card, int index)
        {
            if (index >= CardsInBoardMax) return;
            if (_cardsInBoard.Value[index] != null) return;
            var newList = new List<MinionCardData>();
    
            for (int i = 0; i < CardsInBoardMax; i++)
            {
                if (i < _cardsInBoard.Value.Count)
                    newList.Add(_cardsInBoard.Value[i]);
                else newList.Add(null);
            }
    
            newList[index] = card;
    
            _cardsInBoard.Value = newList;
        }
        
        public void InitBoard()
        {
            var list = new List<MinionCardData>(CardsInBoardMax);
            for (int i = 0; i < CardsInBoardMax; i++)
            {
                list.Add(null);
            }
            _cardsInBoard.Value = list;
        }
        
        public void RemoveCardFromBoard(MinionCardData card)
        {
            var currentList = new List<MinionCardData>(_cardsInBoard.Value);
            currentList.Remove(card);
            _cardsInBoard.Value = currentList;
        }
        
        public void ClearBoard() => _cardsInBoard.Value = new List<MinionCardData>();
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