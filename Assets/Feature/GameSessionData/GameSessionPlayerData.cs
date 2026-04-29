using System;
using System.Collections.Generic;
using Feature.GoogleSheets;
using Feature.Items.Scripts;
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
        
        public List<SpellCardData> HeroPowers;
        
        public bool HeroPowerUsedThisTurn;
        
        public PlayerInventory Inventory { get; } = new PlayerInventory();
        
        public IEnumerable<CardAndHealthEntityOwnerData> GetAllEntityOwners()
        {
            return CardAndHealthEntityOwners;
        }
        
        public SpellCardData CurrentHeroPower => HeroPowers?[0];
        
        public List<MinionCardData> CardsInBoardList
        {
            get => _cardsInBoard.Value;
            set => _cardsInBoard.Value = value;
        }
        
        public bool IsPlayerFirst;
        
        public int _heroPowerCost;

        private readonly ReactiveProperty<int> _currencyCount = new();
        public ReadOnlyReactiveProperty<int> CurrencyCount => _currencyCount;
        
        public int Currency
        {
            get => _currencyCount.Value;
            set => _currencyCount.Value = value;
        }

        public event Action OnHeroPowerReset;
        

        public Sprite _iconImage;
        public Sprite _heroPowerSprite;

        public bool PlayerHasHero()
        {
            return MainHeroEntity()._heroName != null && 
                   MainHeroEntity().HealthValue > 0;
        }
        
        public void AddCardToBoard(MinionCardData card, int index)
        {
            if (index < 0 || index >= CardsInBoardMax) return;
    
            var newList = new List<MinionCardData>(_cardsInBoard.Value);
    
            while (newList.Count <= index)
                newList.Add(null);
    
            if (newList[index] != null) return;
    
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
            int index = currentList.IndexOf(card);
    
            if (index >= 0)
                currentList[index] = null; 
    
            _cardsInBoard.Value = currentList;
        }
        
        public void ClearBoard() => _cardsInBoard.Value = new List<MinionCardData>();
    }

    public class HeroPowerData
    {
        private int cost;
        
    }
}