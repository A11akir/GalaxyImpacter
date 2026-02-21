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
        private BattlefieldSystem _battlefieldSystem;
        private GameSessionModel _gameSessionModel;
        private HandFillSystem _handFillSystem;
        private HandCardPresenter _handCardPresenter;
        private CardCastSystem _cardCastSystem;

        public HandDataRepository(
            CardCastSystem cardCastSystem, 
            HandCardPresenter handCardPresenter, 
            HandFillSystem handFillSystem, GameSessionModel gameSessionModel, BattlefieldSystem battlefieldSystem)
        {
            _cardCastSystem = cardCastSystem;
            _handCardPresenter = handCardPresenter;
            _handFillSystem = handFillSystem;
            _gameSessionModel = gameSessionModel;
            _battlefieldSystem = battlefieldSystem;
        }

        private int _previousCardCount;

        public void Init()
        {
            _gameSessionModel.PlayerHero.CardsInHand
                .Select(x => x.Count)
                .DistinctUntilChanged()
                .Subscribe(count =>
                {
                    if (count > _previousCardCount)
                    {
                        var cardsList = _gameSessionModel.PlayerHero.CardsInHand.CurrentValue;

                        if (cardsList.Count > 0)
                        {
                            var lastCard = cardsList[^1];
                            AddCardToHand(lastCard);
                        }
                    }
                    else if (count < _previousCardCount)
                    {
                        RemoveLastCardFromHand();
                    }
                    
                    _previousCardCount = count;
                });
        }

        private void RemoveLastCardFromHand()
        {
            int removedIndex = _gameSessionModel.PlayerHero.LastRemovedCardIndex;
    
            if (removedIndex < 0 || removedIndex >= _handData.Count) return;
    
            var removedCardData = _handData[removedIndex];
    
            if (removedCardData.Behaviour != null && removedCardData.Logic != null)
            {
                removedCardData.Behaviour.OnTryCardCast -= removedCardData.Logic.CastCard;
            }
    
            if (removedCardData.Behaviour != null)
            {
                Object.Destroy(removedCardData.Behaviour as MonoBehaviour);
                removedCardData.Behaviour = null;
            }
    
            _handCardPresenter.UpdateAfterRemoveCard(removedIndex);
    
            _handData.RemoveAt(removedIndex);
        }

        private void AddCardToHand(CardStatsData card)
        {
            Debug.Log("AddCardToHand");
            var view = _handCardPresenter.AddCardFromHand(card);

            var handCardData = new HandCardData(
                data: card,
                view: view,
                behaviour: null,
                logic: null,
                index : _gameSessionModel.PlayerHero.CardsInHand.CurrentValue.Count
            );
             
            SetupCardBehavioursAndLogic(handCardData);
            _handData.Add(handCardData);
        }

        private void SetupCardBehavioursAndLogic(HandCardData handCardData)
        {
            _cardCastSystem.AddBehavioursToCard(handCardData);
    
            var logic = new GameplayLogicCard(handCardData, _gameSessionModel, _battlefieldSystem);
    
            handCardData.Behaviour.OnTryCardCast += logic.CastCard;
    
            handCardData.Logic = logic;
        }
    }
}