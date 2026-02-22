using System.Collections.Generic;
using System.Linq;
using Feature.Battlefield.Script.View;
using Feature.Card.Script;
using Feature.GameSessionData;
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

        private List<CardStatsData> _previousCardsHero = new();

        [SerializeField] private GameObject _enemyBattlefield;
        [SerializeField] private GameObject _playerBattlefield;

        private List<CardOnBattlefieldView> _cardsGameObjectsEnemy;
        private List<CardOnBattlefieldView> _cardsGameObjectsPlayer;

        [Inject]
        public void Construct(GameSessionModel gameSessionModel,
            CardOnBattlefieldPresenter cardOnBattlefieldPresenter,
            TipPlaceBattlefieldViewSystem tipPlaceBattlefieldViewSystem)
        {
            _gameSessionModel = gameSessionModel;
            _cardOnBattlefieldPresenter = cardOnBattlefieldPresenter;
            _tipPlaceBattlefieldViewSystem = tipPlaceBattlefieldViewSystem;
        }

        public void Init()
        {
            _cardsGameObjectsEnemy = GetCardViewsFromBattlefield(_enemyBattlefield);
            _cardsGameObjectsPlayer = GetCardViewsFromBattlefield(_playerBattlefield);

            SubscribeReactiveBoardList();
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

        private void SubscribeReactiveBoardList()
        {
            _gameSessionModel.PlayerHero.CardsInBoard
                .Subscribe(currentHeroCards =>
                {
                    var nonNullCards = currentHeroCards.Where(c => c != null).ToList();
                    var previousNonNullCards = _previousCardsHero.Where(c => c != null).ToList();

                    var addedCards = nonNullCards
                        .Where(c => previousNonNullCards.All(p => p.id != c.id))
                        .ToList();

                    var removedCards = previousNonNullCards
                        .Where(p => nonNullCards.All(c => c.id != p.id))
                        .ToList();

                    if (removedCards.Count > 0)
                    {
                        var removedCard = removedCards[0];
                        OnCardRemovedFromBoard(removedCard);
                    }

                    if (addedCards.Count > 0)
                    {
                        var addedCard = addedCards[0];
                        int addedIndex = currentHeroCards.FindIndex(c => c != null && c.id == addedCard.id);
                        OnCardAddedBoard(addedCard, addedIndex);
                    }

                    _previousCardsHero = currentHeroCards.ToList();
                });
        }


        private void OnCardAddedBoard(CardStatsData addedCard, int addedIndex)
        {
            _cardOnBattlefieldPresenter.SetCardInPlayerHand(_cardsGameObjectsPlayer[addedIndex], addedCard);
            _gameSessionModel.PlayerHero.RemoveCardFromHand(addedCard);
        }

        private void OnCardRemovedFromBoard(CardStatsData removedCard)
        {
            
        }


        public void AddCardInBattlefield(GameSessionPlayerData playerData, CardStatsData cardData)
        {
            if (_gameSessionModel.PlayerHero == playerData)
            {
                Debug.Log(_tipPlaceBattlefieldViewSystem.GetCardIndex());
                playerData.AddCardToBoard(cardData, _tipPlaceBattlefieldViewSystem.GetCardIndex());
            }
        }
    }
}