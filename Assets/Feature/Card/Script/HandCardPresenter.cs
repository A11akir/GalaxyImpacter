using System.Collections.Generic;
using Feature.GameSessionData;

namespace Feature.Card.Script
{
    public class HandCardPresenter
    {
        private HandCardViews _handCardViews;
        private GameSessionModel _gameSessionModel;
        
        public List<(CardStatsData data, CardView view)> _handCards = 
            new List<(CardStatsData, CardView)>();

        public HandCardPresenter(HandCardViews handCardViews, GameSessionModel gameSessionModel)
        {
            _handCardViews = handCardViews;
            _gameSessionModel = gameSessionModel;
        }
        
        public void SetCardInPlayerHand()
        {
            _handCards.Clear();
            
            var cardsInHand = _gameSessionModel.PlayerHero._cardsInHand;
            var cardViews = _handCardViews._cardsInDeck;
            
            for (int i = 0; i < cardsInHand.Count; i++)
            {
                _handCards.Add((cardsInHand[i], cardViews[i]));
            }
            
            _handCardViews.SetCardsPlayerView(cardsInHand);
        }
    }
}