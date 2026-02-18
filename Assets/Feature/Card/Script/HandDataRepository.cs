using System.Collections.Generic;

namespace Feature.Card.Script
{
    public class HandDataRepository
    {
        public List<HandCardData> _handData = new List<HandCardData>();
        private HandFillSystem _handFillSystem { get;  }
        private HandCardPresenter _handCardPresenter { get;  }
        
        private CardCastSystem _cardCastSystem;

        public HandDataRepository(CardCastSystem cardCastSystem, HandCardPresenter handCardPresenter, HandFillSystem handFillSystem)
        {
            _cardCastSystem = cardCastSystem;
            _handCardPresenter = handCardPresenter;
            _handFillSystem = handFillSystem;
        }

        public void InitPropertyCard()
        {
            _handFillSystem.FillHandDataInDecks();
            _handCardPresenter.SetCardInPlayerHand();
            _cardCastSystem.InitPropertyCard();

            
        }
    }
}