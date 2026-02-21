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

        public HandCardView AddCardFromHand(CardStatsData cardStatsData, int addedIndex)
        {
            return _handCardViews.AddCardFromHand(cardStatsData, addedIndex);
        }

        public void RemoveCardFromHand(HandCardView view)
        {
            _handCardViews.RemoveHandCardView(view);
        }
    }
}