using System.Collections.Generic;
using System.Linq;
using Feature.Battlefield.Script;
using Feature.GameSessionData;
using R3;
using UnityEngine;

namespace Feature.Card.Script
{
    public class HandDataRepository
    {
        public List<HandCardData> _handData = new List<HandCardData>();
        private List<CardStatsData> _previousCards = new();
        private BattlefieldSystem _battlefieldSystem;
        private GameSessionModel _gameSessionModel;
        private HandCardPresenter _handCardPresenter;
        private CardCastSystem _cardCastSystem;

        public HandDataRepository(
            CardCastSystem cardCastSystem,
            HandCardPresenter handCardPresenter, GameSessionModel gameSessionModel, BattlefieldSystem battlefieldSystem)
        {
            _cardCastSystem = cardCastSystem;
            _handCardPresenter = handCardPresenter;
            _gameSessionModel = gameSessionModel;
            _battlefieldSystem = battlefieldSystem;
        }



        public void Init()
        {
            _gameSessionModel.PlayerHero.CardsInHand
                .Subscribe(currentCards =>
                {
                    var addedCards = currentCards
                        .Where(c => !_previousCards.Any(p => p.id == c.id))
                        .ToList();

                    var removedCards = _previousCards
                        .Where(p => !currentCards.Any(c => c.id == p.id))
                        .ToList();

                    if (removedCards.Count > 0)
                    {
                        var removedCard = removedCards[0];
                        int removedIndex = _previousCards.FindIndex(p => p.id == removedCard.id);
                        OnCardRemovedFromHand(removedCard, removedIndex);
                    }

                    if (addedCards.Count > 0)
                    {
                        var addedCard = addedCards[0];
                        int addedIndex = currentCards.FindIndex(c => c.id == addedCard.id);
                        OnCardAddedToHand(addedCard, addedIndex);
                    }

                    _previousCards = currentCards.ToList();
                });
        }

        private void OnCardAddedToHand(CardStatsData addedCard, int addedIndex)
        {
            var view = _handCardPresenter.AddCardFromHand(addedCard, addedIndex);

            var handCardData = new HandCardData(
                data: addedCard,
                view: view,
                behaviour: null,
                logic: null);
            
            _handData.Insert(addedIndex, handCardData);
            
            SetupCardBehavioursAndLogic(addedIndex);
            
        }

        private void OnCardRemovedFromHand(CardStatsData removedCard, int removedIndex)
        {
            var cardToRemove = _handData.FirstOrDefault(c => c.Data.id == removedCard.id);
    
            if (cardToRemove != null)
            {
                _handCardPresenter.RemoveCardFromHand(cardToRemove.View);
        
                _cardCastSystem.RemoveBehaviourFromCard(cardToRemove);
        
                _handData.Remove(cardToRemove);
            }
        }
        private void SetupCardBehavioursAndLogic(int index)
        {
            _cardCastSystem.AddBehavioursToCard(_handData[index]);

            var logic = new GameplayLogicCard(_handData[index], _gameSessionModel, _battlefieldSystem);

            _handData[index].Behaviour.OnTryCardCast += logic.CastCard;

            _handData[index].Logic = logic;
        }
    }
}