using Feature.Card.Script;
using Feature.ShopGamePlay.Script;
using Feature.ShopGamePlay.Script.Currency;
using UnityEngine;

namespace Feature.GameSessionFSM
{
    public class TurnСycleGameSessionSystem
    {
        private DeckFillSystem _deckFillSystem { get;  }
        private HandFillSystem _handFillSystem { get;  }
        private HandCardPresenter _handCardPresenter { get;  }
        
        private CurrencyManagerSystem _currencyManagerSystem { get;  }
        
        public TurnСycleGameSessionSystem(DeckFillSystem deckFillSystem, HandFillSystem handFillSystem,
            HandCardPresenter handCardPresenter, CurrencyManagerSystem currencyManagerSystem)
        {
            _deckFillSystem = deckFillSystem;
            _handFillSystem = handFillSystem;
            _handCardPresenter = handCardPresenter;
            _currencyManagerSystem = currencyManagerSystem;
        }

        public void StartGameSession()
        {
            _deckFillSystem.InitializeDecks();
            _handFillSystem.FillHandInDecks();
            _handCardPresenter.SetCardInPlayerHand();
            _currencyManagerSystem.Init();
        }

        public void CycleTurn()
        {
            _currencyManagerSystem.NewTurnUpdate();
        }
    }
}