using Feature.Battlefield.Script;
using Feature.Card.Script;
using Feature.Chakra;
using Feature.ShopGamePlay.Script.Currency;

namespace Feature.GameSessionFSM
{
    public class TurnСycleGameSessionSystem
    {
        private HandFillSystem _handFillSystem;
        private BattlefieldSystem _battlefieldSystem;
        private DeckFillSystem _deckFillSystem { get;  }
        private HandDataRepository _handDataRepository { get;  }
        private CurrencyManagerSystem _currencyManagerSystem { get;  }        
        private ChakraManagerSystem _chakraManagerSystem { get;  }
        
        public TurnСycleGameSessionSystem(DeckFillSystem deckFillSystem, CurrencyManagerSystem currencyManagerSystem,
            ChakraManagerSystem chakraManagerSystem, HandDataRepository handDataRepository, BattlefieldSystem battlefieldSystem, HandFillSystem handFillSystem)
        {
            _deckFillSystem = deckFillSystem;
            _currencyManagerSystem = currencyManagerSystem;
            _chakraManagerSystem = chakraManagerSystem;
            _handDataRepository = handDataRepository;
            _battlefieldSystem = battlefieldSystem;
            _handFillSystem = handFillSystem;
        }
        

        public void StartGameSession()
        {
            _deckFillSystem.InitializeDecks();
            _handDataRepository.Init();
            _chakraManagerSystem.Init();
            _battlefieldSystem.Init();
            _currencyManagerSystem.Init();
        }

        public void CycleTurn()
        {
            _handFillSystem.FillHandDataInDecks();
            _currencyManagerSystem.NewTurnUpdate();
            _chakraManagerSystem.NewTurnUpdate();
        }
    }
}