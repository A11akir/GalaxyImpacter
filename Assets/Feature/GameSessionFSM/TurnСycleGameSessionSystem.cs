using Feature.Card.Script;
using Feature.ShopGamePlay.Script;

namespace Feature.GameSessionFSM
{
    public class TurnСycleGameSessionSystem
    {
        private DeckFillSystem _deckFillSystem { get;  }
        private HandFillSystem _handFillSystem { get;  }
        private HandCardPresenter _handCardPresenter { get;  }
        
        private CurrencyManagerSystem _currencyManagerSystem { get;  }
        
        public TurnСycleGameSessionSystem(DeckFillSystem deckFillSystem, HandFillSystem handFillSystem,
            HandCardPresenter handCardPresenter)
        {
            _deckFillSystem = deckFillSystem;
            _handFillSystem = handFillSystem;
            _handCardPresenter = handCardPresenter;
        }

        public void StartGameSession()
        {
            _deckFillSystem.InitializeDecks();
            _handFillSystem.FillHandInDecks();
            _handCardPresenter.SetCardInPlayerHand();
        }

        public void CycleTurn()
        {
            _currencyManagerSystem.NewTurnUpdate();
        }
    }
}