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
        
        public List<HandCardView> GetHandViews()
        {
            return _handCardViews._cardsInHand;
            
        }
        
        public void SetCardInPlayerHand()
        {
            Debug.Log("SetCardInPlayerHand");

            var cardsInHand = _gameSessionModel.PlayerHero.CardsInHand.CurrentValue;

            _handCardViews.SetCardsPlayerView(cardsInHand);
        }
        
        public HandCardView GetCardView(int index)
        {
            if (index >= 0 && index < _handCardViews._cardsInHand.Count)
                return _handCardViews._cardsInHand[index];
                
            return null;
        }

        public void ChakraCheckCanCastCard(List<HandCardData> handCardData)
        {
            foreach (var cardData in handCardData)
            {
                cardData.View.SetCanCastView(_gameSessionModel.PlayerHero.Chakra >= cardData.Data.Cost);
            }
        }
    }
}