using System.Collections.Generic;
using System.Linq;
using Feature.Battlefield.Script;
using Feature.GameSessionData;
using Feature.HandLogic;
using R3;
using UnityEngine;

namespace Feature.Card.Script
{
    public class HandDataRepository
    {
        private readonly Dictionary<CardAndHealthEntityOwnerData, EntityHandState> _entityHands = new();
    
        private HandCardsPositionSystem  _handCardsPositionSystem;
        private readonly CardCastService _cardCastService;
        private readonly HandCardPresenter _handCardPresenter;
        private readonly FactoryHandBehaviourTransformCastSystem _factoryHandBehaviourTransformCastSystem;

        public HandDataRepository(
            FactoryHandBehaviourTransformCastSystem factoryHandBehaviourTransformCastSystem,
            HandCardPresenter handCardPresenter, HandCardsPositionSystem handCardsPositionSystem,
            CardCastService cardCastService)
        {
            _factoryHandBehaviourTransformCastSystem = factoryHandBehaviourTransformCastSystem;
            _handCardPresenter = handCardPresenter;
            _handCardsPositionSystem = handCardsPositionSystem;
            _cardCastService = cardCastService;
        }

        public void InitHandRepository(CardAndHealthEntityOwnerData owner, HandCardViews handCardViews, bool isHidden = false)
        {
            if (_entityHands.ContainsKey(owner)) return;
            var state = new EntityHandState(owner, handCardViews, isHidden);
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
            }).AddTo(state.Disposables);
        }

// HandDataRepository.cs
        private void OnCardAddedToHand(CardStatsData addedCard, int addedIndex, EntityHandState state)
        {
            var view = state.IsHiddenForEnemyPlayer
                ? state.HandCardViews.AddCardAsHiddenForEnemyPlayer(addedIndex)
                : state.HandCardViews.AddCardFromHand(addedCard, addedIndex);

            var handCardData = new HandCardData(data: addedCard, view: view, behaviour: null, logic: null);
            state.HandData.Insert(addedIndex, handCardData);

            if (!state.IsHiddenForEnemyPlayer)
            {
                SetupHandCardBehavioursAndLogic(addedIndex, state);

                _handCardPresenter.ActivatePassiveEffects(
                    view,
                    addedCard,
                    state.Owner,
                    state.HandData, // ← вся рука целиком
                    _factoryHandBehaviourTransformCastSystem); // ← у тебя уже есть это поле в HandDataRepository
            }

            _handCardsPositionSystem.UpdateCardsPosition(state.HandCardViews.transform);
        }
        

        private void OnCardRemovedFromHand(CardStatsData removedCard, EntityHandState state)
        { var cardToRemove = state.HandData.FirstOrDefault(c => c.Data.id == removedCard.id);
            if (cardToRemove == null) return;

            _handCardPresenter.RemoveCardFromHand(cardToRemove.View, state.HandCardViews);
            _factoryHandBehaviourTransformCastSystem.RemoveBehaviourFromHandCard(cardToRemove);
            state.HandData.Remove(cardToRemove);
    
            _handCardsPositionSystem.UpdateCardsPosition(state.HandCardViews.transform);
        }

        private void SetupHandCardBehavioursAndLogic(int index, EntityHandState state)
        {
            _factoryHandBehaviourTransformCastSystem.AddBehavioursToCard(state.HandData[index]);

            state.HandData[index].Behaviour.SetOwner(state.Owner);
    
            bool canCast = state.Owner.Chakra >= state.HandData[index].Data.Cost;
            state.HandData[index].Behaviour.CanCastCard(canCast);
            _handCardPresenter.ChakraCheckCanCastCard(state.HandData[index], state.Owner.Chakra);
    
            var logic = new HandCardCastHandler(state.HandData[index], _cardCastService);
            state.HandData[index].Behaviour.OnTryCardCast += logic.CastCard;
            state.HandData[index].Logic = logic;
        }
        
        public void DisposeOwner(CardAndHealthEntityOwnerData owner)
        {
            if (_entityHands.TryGetValue(owner, out var state))
            {
                state.Disposables.Dispose();
                _entityHands.Remove(owner);
            }
        }
    }
}