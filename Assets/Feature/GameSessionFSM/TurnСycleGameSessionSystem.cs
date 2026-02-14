using Feature.Card.Script;

namespace Feature.GameSessionFSM
{
    public class TurnСycleGameSessionSystem
    {
        private DeckFillSystem _deckFillSystem { get;  }
        private HandFillSystem _handFillSystem { get;  }
        private HandCardPresenter _handCardPresenter { get;  }
        
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
            
        }
    }
}