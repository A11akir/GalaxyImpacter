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
        private readonly Dictionary<CardAndHealthEntityOwnerData, EntityHandState> _entityHands = new();
    
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

        public void InitHandRepository(CardAndHealthEntityOwnerData owner, HandCardViews handCardViews)
        {
            if (_entityHands.ContainsKey(owner)) return;
            var state = new EntityHandState(owner, handCardViews);
            _entityHands[owner] = state;
            SubscribeReactiveHandList(state);
        }

        public List<HandCardData> GetHandData(CardAndHealthEntityOwnerData owner)
            => _entityHands.TryGetValue(owner, out var state) ? state.HandData : null;  

        private void SubscribeReactiveHandList(EntityHandState state)
        {
            state.Owner.CardsInHand.Subscribe(currentCards =>
            {
                var previous = state.PreviousCards;
                state.PreviousCards = currentCards.ToList();

                var addedCards = currentCards
                    .Where(c => !previous.Any(p => p.id == c.id))
                    .ToList();

                var removedCards = previous
                    .Where(p => !currentCards.Any(c => c.id == p.id))
                    .ToList();

                foreach (var removedCard in removedCards)
                    OnCardRemovedFromHand(removedCard, state);

                for (int i = 0; i < currentCards.Count; i++)
                {
                    var card = currentCards[i];
                    if (addedCards.All(addedCard => addedCard.id != card.id)) continue;
                    OnCardAddedToHand(card, i, state);
                }
            });
        }

        private void OnCardAddedToHand(CardStatsData addedCard, int addedIndex, EntityHandState state)
        {
            var view = state.HandCardViews.AddCardFromHand(addedCard, addedIndex); 

            var handCardData = new HandCardData(
                data: addedCard,
                view: view,
                behaviour: null,
                logic: null);

            state.HandData.Insert(addedIndex, handCardData);
            SetupHandCardBehavioursAndLogic(addedIndex, state);
        }

        private void OnCardRemovedFromHand(CardStatsData removedCard, EntityHandState state)
        {
            var cardToRemove = state.HandData.FirstOrDefault(c => c.Data.id == removedCard.id);
            if (cardToRemove == null) return;

            _handCardPresenter.RemoveCardFromHand(cardToRemove.View, state.HandCardViews);
            _cardCastSystem.RemoveBehaviourFromHandCard(cardToRemove);
            state.HandData.Remove(cardToRemove);
        }

        private void SetupHandCardBehavioursAndLogic(int index, EntityHandState state)
        {
            _cardCastSystem.AddBehavioursToCard(state.HandData[index]);

            state.HandData[index].Behaviour.SetOwner(state.Owner);
    
            bool canCast = state.Owner.Chakra >= state.HandData[index].Data.Cost;
            state.HandData[index].Behaviour.CanCastCard(canCast);
            _handCardPresenter.ChakraCheckCanCastCard(state.HandData[index], state.Owner.Chakra);
    
            var logic = new GameplayLogicCard(state.HandData[index], _gameSessionModel, _battlefieldSystem);
            state.HandData[index].Behaviour.OnTryCardCast += logic.CastCard;
            state.HandData[index].Logic = logic;
        }
    }
}