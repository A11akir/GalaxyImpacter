using System;
using System.Collections.Generic;
using Feature.GameSessionData;

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
        
        public List<CardView> GetHandViews()
        {
            return _handCardViews._cardsInDeck;
            
        }
        
        public void SetCardInPlayerHand()
        {
            var cardsInHand = _gameSessionModel.PlayerHero._cardsInHand;
            _handCardViews.SetCardsPlayerView(cardsInHand);
        }
        
        public CardView GetCardView(int index)
        {
            if (index >= 0 && index < _handCardViews._cardsInDeck.Count)
                return _handCardViews._cardsInDeck[index];
                
            return null;
        }

        public void ChakraCheckCanCastCard(List<HandCardData> handCardData)
        {
            foreach (var cardData in handCardData)
            {
                cardData.View.SetCanCastView(_gameSessionModel.PlayerHero.Chakra > cardData.Data.Cost);
            }
        }
    }
}