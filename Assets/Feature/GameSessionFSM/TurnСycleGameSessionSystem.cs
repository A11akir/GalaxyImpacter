using Feature.Battlefield.Script;
using Feature.Card.Script;
using Feature.Chakra;
using Feature.GameSessionData;
using Feature.Hero;
using Feature.ShopGamePlay.Script.Currency;
using Feature.UI;

namespace Feature.GameSessionFSM
{
    public class TurnСycleGameSessionSystem
    {
        private CreateOwnerCardAndHealthEntitySystem _createOwnerCardAndHealthEntitySystem;
        private readonly HandFillSystem _handFillSystem;
        private readonly BattlefieldSystem _battlefieldSystem;
        private GameSessionModel _gameSessionModel;
        private DeckFillSystem _deckFillSystem { get;  }
        private HandDataRepository _handDataRepository { get;  }
        private CurrencyManagerSystem _currencyManagerSystem { get;  }        
        private ChakraManagerSystem _chakraManagerSystem { get;  }
        
        private GameSessionPresenter _gameSessionPresenter { get;  }
        
        public TurnСycleGameSessionSystem(DeckFillSystem deckFillSystem, CurrencyManagerSystem currencyManagerSystem,
            ChakraManagerSystem chakraManagerSystem, HandDataRepository handDataRepository, BattlefieldSystem battlefieldSystem, HandFillSystem handFillSystem, GameSessionModel gameSessionModel, CreateOwnerCardAndHealthEntitySystem createOwnerCardAndHealthEntitySystem, GameSessionPresenter gameSessionPresenter)
        {
            _deckFillSystem = deckFillSystem;
            _currencyManagerSystem = currencyManagerSystem;
            _chakraManagerSystem = chakraManagerSystem;
            _handDataRepository = handDataRepository;
            _battlefieldSystem = battlefieldSystem;
            _handFillSystem = handFillSystem;
            _gameSessionModel = gameSessionModel;
            _createOwnerCardAndHealthEntitySystem = createOwnerCardAndHealthEntitySystem;
            _gameSessionPresenter = gameSessionPresenter;
        }

        public void StartGameSession()
        {
            _gameSessionPresenter.SetupEntityViews(); // ← вместо CreatePlayersEntity напрямую
            _gameSessionModel.PlayerHero.InitBoard();
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