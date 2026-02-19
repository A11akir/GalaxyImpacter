using Feature.Card.Script;
using Feature.Chakra;
using Feature.ShopGamePlay.Script;
using Feature.ShopGamePlay.Script.Currency;
using UnityEngine;

namespace Feature.GameSessionFSM
{
    public class TurnСycleGameSessionSystem
    {
        private DeckFillSystem _deckFillSystem { get;  }
        private HandDataRepository _handDataRepository { get;  }
        private CurrencyManagerSystem _currencyManagerSystem { get;  }        
        private ChakraManagerSystem _chakraManagerSystem { get;  }
        
        public TurnСycleGameSessionSystem(DeckFillSystem deckFillSystem, CurrencyManagerSystem currencyManagerSystem,
            ChakraManagerSystem chakraManagerSystem, HandDataRepository handDataRepository)
        {
            _deckFillSystem = deckFillSystem;
            _currencyManagerSystem = currencyManagerSystem;
            _chakraManagerSystem = chakraManagerSystem;
            _handDataRepository = handDataRepository;
        }

        public void StartGameSession()
        {
            _deckFillSystem.InitializeDecks();
            _currencyManagerSystem.Init();
            _chakraManagerSystem.Init();
        }

        public void CycleTurn()
        {
            _handDataRepository.InitPropertyCard();
            _currencyManagerSystem.NewTurnUpdate();
            _chakraManagerSystem.NewTurnUpdate();
        }
    }
}