using Feature.Battlefield.Script;
using Feature.Card.Script;
using Feature.Chakra;
using Feature.GameSessionData;
using Feature.ShopGamePlay.Script.Currency;
using Feature.ShopGamePlay.Script.ShopWindow;
using Feature.StagesGameLogic;
using Feature.Timer;
using Feature.UI;

namespace Feature.GameSessionFSM
{
    public class TurnСycleGameSessionSystem
    {
        private readonly HandFillSystem _handFillSystem;
        private readonly BattlefieldSystem _battlefieldSystem;
        private readonly GameSessionModel _gameSessionModel;
        private readonly TimerStageGameSessionSystem _timerStageGameSessionSystem;
        private readonly ReadyStageBackOrFightSystem _readyStageBackOrFightSystem;
        private DeckFillSystem _deckFillSystem { get;  }
        
        private PrepareStatePresenter _prepareStatePresenter { get;  }
        private FightStatePresenter  _fightStatePresenter { get;  }
        private HandDataRepository _handDataRepository { get;  }
        private CurrencyManagerSystem _currencyManagerSystem { get;  }        
        private ChakraManagerSystem _chakraManagerSystem { get;  }
        
        private ShopGameplayManagerSystem _shopSystem { get;  }
        private GameSessionPresenter _gameSessionPresenter { get;  }
        
        public TurnСycleGameSessionSystem(DeckFillSystem deckFillSystem, CurrencyManagerSystem currencyManagerSystem,
            ChakraManagerSystem chakraManagerSystem, HandDataRepository handDataRepository, BattlefieldSystem battlefieldSystem,
            HandFillSystem handFillSystem, GameSessionModel gameSessionModel, GameSessionPresenter gameSessionPresenter, ShopGameplayManagerSystem shopSystem, FightStatePresenter fightStatePresenter, PrepareStatePresenter prepareStatePresenter, ReadyStageBackOrFightSystem readyStageBackOrFightSystem, TimerStageGameSessionSystem timerStageGameSessionSystem)
        {
            _deckFillSystem = deckFillSystem;
            _currencyManagerSystem = currencyManagerSystem;
            _chakraManagerSystem = chakraManagerSystem;
            _handDataRepository = handDataRepository;
            _battlefieldSystem = battlefieldSystem;
            _handFillSystem = handFillSystem;
            _gameSessionModel = gameSessionModel;
            _gameSessionPresenter = gameSessionPresenter;
            _shopSystem = shopSystem;
            _fightStatePresenter = fightStatePresenter;
            _prepareStatePresenter = prepareStatePresenter;
            _readyStageBackOrFightSystem = readyStageBackOrFightSystem;
            _timerStageGameSessionSystem = timerStageGameSessionSystem;
        }

        public void StartGameSession()
        {
            _gameSessionPresenter.SetupEntityViews();
            _gameSessionModel.PlayerHero.InitBoard();
            _battlefieldSystem.Init();
            _currencyManagerSystem.Init();
            _readyStageBackOrFightSystem.SetEnemyReady();
            _currencyManagerSystem.NewTurnUpdate();
            _prepareStatePresenter.StartPrepare();
            _timerStageGameSessionSystem.StartTimerPrepare(_gameSessionModel.Turn);
            _shopSystem.UnlockShop();
        }

        public void CycleStartPrepareTurn()
        {
            _readyStageBackOrFightSystem.Reset();
            _readyStageBackOrFightSystem.SetEnemyReady();
            _prepareStatePresenter.StartPrepare();
            _handFillSystem.FillHandDataInDecks();
            _currencyManagerSystem.NewTurnUpdate();
            _chakraManagerSystem.NewTurnUpdate();
            _shopSystem.UnlockShop();
            _timerStageGameSessionSystem.StartTimerPrepare(_gameSessionModel.Turn);
        }

        public void CycleStartFightTurn()
        {
            _readyStageBackOrFightSystem.Reset();
            _readyStageBackOrFightSystem.SetEnemyReady();
            _shopSystem.LockShop();
            _fightStatePresenter.StartFight();
            _timerStageGameSessionSystem.StartTimerFight(_gameSessionModel.Turn);
        }

        public void CycleEndFightTurn()
        {
            _fightStatePresenter.EndFight();
        }

        public void CycleEndPrepareTurn()
        {
            _prepareStatePresenter.EndPrepare();
        }
    }
}