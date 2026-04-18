using System.Collections.Generic;
using System.Linq;
using Feature.Battlefield.Script.View;
using Feature.Card.Script;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using Feature.HandLogic;
using Feature.Hero;
using R3;
using UnityEngine;
using Zenject;

namespace Feature.Battlefield.Script
{
    public class BattlefieldSystem : MonoBehaviour
    {
        private TipPlaceBattlefieldViewSystem _tipPlaceBattlefieldViewSystem;
        private CardOnBattlefieldPresenter _cardOnBattlefieldPresenter;
        private GameSessionModel _gameSessionModel;
        private CreateOwnerCardAndHealthEntitySystem _createOwnerCardAndHealthEntitySystem;
        private TargetingSystem _targetingSystem;

        [SerializeField] private GameObject enemyBattlefield;
        [SerializeField] private GameObject playerBattlefield;
                
        private readonly Dictionary<GameSessionPlayerData, List<CardOnBattlefieldView>> _battlefieldViews = new();
        private readonly Dictionary<GameSessionPlayerData, List<MinionCardData>> _previousCards = new();
        
        private readonly Dictionary<CardAndHealthEntityOwnerData, CardOnBattlefieldView> _ownerToView = new();
        
        private HandViewSwitcher _handViewSwitcher;

        [Inject]
        public void Construct(GameSessionModel gameSessionModel,
            CardOnBattlefieldPresenter cardOnBattlefieldPresenter,
            TipPlaceBattlefieldViewSystem tipPlaceBattlefieldViewSystem,
            CreateOwnerCardAndHealthEntitySystem createOwnerCardAndHealthEntitySystem,
            HandViewSwitcher handViewSwitcher, TargetingSystem targetingSystem)
        {
            _handViewSwitcher = handViewSwitcher;
            _targetingSystem = targetingSystem;
            _tipPlaceBattlefieldViewSystem = tipPlaceBattlefieldViewSystem;
            _cardOnBattlefieldPresenter = cardOnBattlefieldPresenter;
            _gameSessionModel = gameSessionModel;
            _createOwnerCardAndHealthEntitySystem = createOwnerCardAndHealthEntitySystem;
            _handViewSwitcher.OnOwnerSwitched += OnOwnerSwitched;
        }

        public void Init()
        {
            _battlefieldViews[_gameSessionModel.PlayerHero] = GetCardViewsFromBattlefield(playerBattlefield);
            _battlefieldViews[_gameSessionModel.EnemyHero] = GetCardViewsFromBattlefield(enemyBattlefield);

            _previousCards[_gameSessionModel.PlayerHero] = new();
            _previousCards[_gameSessionModel.EnemyHero] = new();
            
            SubscribeReactiveBoardList(_gameSessionModel.PlayerHero);
            SubscribeReactiveBoardList(_gameSessionModel.EnemyHero);
        }

        private List<CardOnBattlefieldView> GetCardViewsFromBattlefield(GameObject battlefield)
        {
            var cardViews = new List<CardOnBattlefieldView>();
            foreach (Transform child in battlefield.transform)
            {
                var cardView = child.GetComponent<CardOnBattlefieldView>();
                cardViews.Add(cardView);
            }
            return cardViews;
        }

        private void SubscribeReactiveBoardList(GameSessionPlayerData playerData)
        {
            playerData.CardsInBoard
                .Subscribe(currentCards =>
                {
                    var nonNullCards = currentCards.Where(c => c != null).ToList();
                    var previousNonNullCards = _previousCards[playerData].Where(c => c != null).ToList();

                    var addedCards = nonNullCards
                        .Where(c => previousNonNullCards.All(p => p.id != c.id))
                        .ToList();

                    var removedCards = previousNonNullCards
                        .Where(p => nonNullCards.All(c => c.id != p.id))
                        .ToList();

                    if (removedCards.Count > 0)
                        OnCardRemovedFromBoard(removedCards[0], playerData);

                    if (addedCards.Count > 0)
                    {
                        int addedIndex = currentCards.FindIndex(c => c != null && c.id == addedCards[0].id);
                        OnCardAddedBoard(addedCards[0], addedIndex, playerData);
                    }

                    _previousCards[playerData] = currentCards.ToList();
                });
        }
        private void OnOwnerSwitched(CardAndHealthEntityOwnerData owner)
        {
            foreach (var kvp in _ownerToView)
                kvp.Value.SetSelected(kvp.Key == owner);
        }

        private readonly Dictionary<CardAndHealthEntityOwnerData, int> _ownerToSlot = new();

        private void OnCardAddedBoard(MinionCardData addedCard, int addedIndex, GameSessionPlayerData playerData)
        {
            var view = SetupBattlefieldView(addedCard, addedIndex, playerData);
            
            var newOwner = CreateOwnerFromCard(addedCard);
            _ownerToSlot[newOwner] = addedIndex;
            _tipPlaceBattlefieldViewSystem.OccupySlot(addedIndex);
            playerData.CardAndHealthEntityOwners.Add(newOwner);
            RegisterOwnerView(newOwner, view);

            _createOwnerCardAndHealthEntitySystem.CreateEntityPlayer(newOwner, view);
        }

        private CardOnBattlefieldView SetupBattlefieldView(MinionCardData addedCard, int addedIndex, GameSessionPlayerData playerData)
        {
            var view = _battlefieldViews[playerData][addedIndex];
            _cardOnBattlefieldPresenter.SetCardInBattlefield(view, addedCard);
            return view;
        }

        private CardAndHealthEntityOwnerData CreateOwnerFromCard(MinionCardData card)
        {
            return new CardAndHealthEntityOwnerData
            {
                CardId = card.id,
                startCardsInDeckCount = card.SpellsList.Count,
                startCardsInHandToDraw = card.HandCardCount,
                _heroName = card.Name,
                HealthValue = card.Health,
                Chakra = card.Chakra,
                _iconImage = card.IconImage,
                SpellsList = card.SpellsList,
                StartChakra = card.Chakra,
            };
        }

        private void RegisterOwnerView(CardAndHealthEntityOwnerData owner, CardOnBattlefieldView view)
        {
            _ownerToView[owner] = view;
            _targetingSystem.RegisterTarget(view.gameObject, owner);
            view.OnClicked += () => _handViewSwitcher.SwitchTo(owner);
        }

        private void OnCardRemovedFromBoard(MinionCardData removedCard, GameSessionPlayerData playerData)
        {
            var owner = _ownerToView.Keys.FirstOrDefault(o => o.CardId == removedCard.id);
            if (owner == null) return;

            var view = _ownerToView[owner];
    
            int slot = _battlefieldViews[playerData].IndexOf(view);
            if (slot >= 0)
                _tipPlaceBattlefieldViewSystem.FreeSlot(slot);

            view.ClearData();

            if (_handViewSwitcher.CurrentOwner == owner)
                _handViewSwitcher.SwitchTo(playerData.MainHeroEntity());

            _ownerToView.Remove(owner);
            _ownerToSlot.Remove(owner);
            playerData.CardAndHealthEntityOwners.Remove(owner);
        }

        public void AddCardInBattlefield(GameSessionPlayerData playerData, CardStatsData cardData) => 
            playerData.AddCardToBoard((MinionCardData)cardData, _tipPlaceBattlefieldViewSystem.GetCardIndex());
    }
}