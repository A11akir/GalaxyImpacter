using System;
using System.Collections.Generic;
using Feature.GameSessionData;
using UnityEngine;

namespace Feature.Card.Script
{
    public class HandCardPresenter
    {
        private HandCardViews _handCardViews;
        private GameSessionModel _gameSessionModel;
        
        public HandCardPresenter(HandCardViews handCardViews, GameSessionModel gameSessionModel)
        {
            _handCardViews = handCardViews;
            _gameSessionModel = gameSessionModel;
        }

        public void ChakraCheckCanCastCard(List<HandCardData> handCardData)
        {
            foreach (var cardData in handCardData)
            {
                cardData.View.SetCanCastView(_gameSessionModel.PlayerHero.Chakra >= cardData.Data.Cost);
            }
        }

        public HandCardView AddCardFromHand(CardStatsData cardStatsData)
        {
            _handCardViews.SetHandCardView(cardStatsData, _gameSessionModel.PlayerHero.CardsInHand.CurrentValue.Count);
            return _handCardViews._cardsInHand[_gameSessionModel.PlayerHero.CardsInHand.CurrentValue.Count];
        }

        public void UpdateAfterRemoveCard(int lastRemovedCardIndex)
        {
            Debug.Log("UpdateAfterRemoveCard: " + lastRemovedCardIndex);
            _handCardViews._cardsInHand[lastRemovedCardIndex].ViewDelete();
        }
    }
}