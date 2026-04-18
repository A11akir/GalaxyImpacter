
using System.Collections.Generic;


namespace Feature.Card.Script
{
    public class HandCardPresenter
    {
        public void ChakraCheckCanCastHand(List<HandCardData> handCardData, int chakra)
        {
            foreach (var cardData in handCardData)
                ChakraCheckCanCastCard(cardData, chakra);
        }

        public void ChakraCheckCanCastCard(HandCardData cardData, int chakra)
        {
            cardData.View.SetCanCastView(chakra >= cardData.Data.Cost);
        }

        public void RemoveCardFromHand(HandCardView view, HandCardViews handCardViews)
        {
            handCardViews.RemoveHandCardView(view);
        }
    }
}