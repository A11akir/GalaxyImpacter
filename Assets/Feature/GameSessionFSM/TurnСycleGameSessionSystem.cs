using Feature.Card.Script;
using Feature.Chakra;
using Feature.ShopGamePlay.Script;
using Feature.ShopGamePlay.Script.Currency;
using UnityEngine;

namespace Feature.GameSessionFSM
{
    public class TurnСycleGameSessionSystem
    {
        private CardCastSystem _cardCastSystem;
        private DeckFillSystem _deckFillSystem { get;  }
        private HandFillSystem _handFillSystem { get;  }
        private HandCardPresenter _handCardPresenter { get;  }
        
        private CurrencyManagerSystem _currencyManagerSystem { get;  }        
        private ChakraManagerSystem _chakraManagerSystem { get;  }
        
        public TurnСycleGameSessionSystem(DeckFillSystem deckFillSystem, HandFillSystem handFillSystem,
            HandCardPresenter handCardPresenter, CurrencyManagerSystem currencyManagerSystem, ChakraManagerSystem chakraManagerSystem, CardCastSystem cardCastSystem)
        {
            _deckFillSystem = deckFillSystem;
            _handFillSystem = handFillSystem;
            _handCardPresenter = handCardPresenter;
            _currencyManagerSystem = currencyManagerSystem;
            _chakraManagerSystem = chakraManagerSystem;
            _cardCastSystem = cardCastSystem;
        }

        public void StartGameSession()
        {
            _deckFillSystem.InitializeDecks();
            _handFillSystem.FillHandInDecks();
            _handCardPresenter.SetCardInPlayerHand();
            _cardCastSystem.InitPropertyCard();
            _currencyManagerSystem.Init();
            _chakraManagerSystem.Init();
        }

        public void CycleTurn()
        {
            _currencyManagerSystem.NewTurnUpdate();
            _chakraManagerSystem.NewTurnUpdate();
        }
    }
}