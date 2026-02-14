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

        public void SetCardInPlayerHand()
        {
            _handCardViews.SetCardsPalyerView(_gameSessionModel.PlayerHero._cardsInHand);
        }
        
    }
}