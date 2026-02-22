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
        public readonly List<HandCardData> HandData = new List<HandCardData>();
        private List<CardStatsData> _previousCards = new();
        private readonly BattlefieldSystem _battlefieldSystem;
        private readonly GameSessionModel _gameSessionModel;
        private readonly HandCardPresenter _handCardPresenter;
        private readonly CardCastSystem _cardCastSystem;

        public HandDataRepository(
            CardCastSystem cardCastSystem, HandCardPresenter handCardPresenter,
            GameSessionModel gameSessionModel, BattlefieldSystem battlefieldSystem)
        {
            _cardCastSystem = cardCastSystem;
            _handCardPresenter = handCardPresenter;
            _gameSessionModel = gameSessionModel;
            _battlefieldSystem = battlefieldSystem;
        }

        public void Init() => SubscribeReactiveHandList();

        private void SubscribeReactiveHandList()
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
                        OnCardRemovedFromHand(removedCard);
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
            
            HandData.Insert(addedIndex, handCardData);
            SetupCardBehavioursAndLogic(addedIndex);
        }

        private void OnCardRemovedFromHand(CardStatsData removedCard)
        {
            var cardToRemove = HandData.FirstOrDefault(c => c.Data.id == removedCard.id);

            if (cardToRemove == null) return;
            
            _handCardPresenter.RemoveCardFromHand(cardToRemove.View);
        
            _cardCastSystem.RemoveBehaviourFromCard(cardToRemove);
        
            HandData.Remove(cardToRemove);
        }
        private void SetupCardBehavioursAndLogic(int index)
        {
            _cardCastSystem.AddBehavioursToCard(HandData[index]);

            var logic = new GameplayLogicCard(HandData[index], _gameSessionModel, _battlefieldSystem);

            HandData[index].Behaviour.OnTryCardCast += logic.CastCard;

            HandData[index].Logic = logic;
        }
    }
}