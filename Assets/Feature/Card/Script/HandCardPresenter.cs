
using System.Collections.Generic;
using Feature.GameSessionData;
using UnityEngine;

namespace Feature.Card.Script
{
    public class HandCardPresenter
    {
        private HandCardViews _handCardViews;
        
        public HandCardPresenter(HandCardViews handCardViews)
        {
            _handCardViews = handCardViews;
        }

        public void ChakraCheckCanCastCard(List<HandCardData> handCardData, int chakra)
        {
            foreach (var cardData in handCardData)
            {
                cardData.View.SetCanCastView(chakra >= cardData.Data.Cost);
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