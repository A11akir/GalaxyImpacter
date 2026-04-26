using Feature.AI;
using Feature.Battlefield.Script;
using Feature.Card.Script;
using Feature.Chakra;
using Feature.GameSessionData;
using Feature.Hero;
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
        private readonly TargetingSystem _targetingSystem;
        private readonly TimerStageGameSessionSystem _timerStageGameSessionSystem;
        private readonly ReadyStageBackOrFightSystem _readyStageBackOrFightSystem;
        private DeckFillSystem _deckFillSystem { get;  }
        private AISystem _aiSystem { get; }
        private PrepareStatePresenter _prepareStatePresenter { get;  }
        private FightStatePresenter  _fightStatePresenter { get;  }
        private HandDataRepository _handDataRepository { get;  }
        private CurrencyManagerSystem _currencyManagerSystem { get;  }        
        private ChakraManagerSystem _chakraManagerSystem { get;  }
        private HeroPowerSystem _heroPowerSystem { get;  }
        private ShopGameplayManagerSystem _shopSystem { get;  }
        private GameSessionPresenter _gameSessionPresenter { get;  }
        
        public TurnСycleGameSessionSystem(DeckFillSystem deckFillSystem, CurrencyManagerSystem currencyManagerSystem,
            ChakraManagerSystem chakraManagerSystem, HandDataRepository handDataRepository, BattlefieldSystem battlefieldSystem,
            HandFillSystem handFillSystem, GameSessionModel gameSessionModel, GameSessionPresenter gameSessionPresenter,
            ShopGameplayManagerSystem shopSystem, FightStatePresenter fightStatePresenter, 
            PrepareStatePresenter prepareStatePresenter, ReadyStageBackOrFightSystem readyStageBackOrFightSystem,
            TimerStageGameSessionSystem timerStageGameSessionSystem, HeroPowerSystem heroPowerSystem, 
            AISystem aiSystem, TargetingSystem targetingSystem)
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
            _heroPowerSystem = heroPowerSystem;
            _aiSystem = aiSystem;
            _targetingSystem = targetingSystem;
        }

        public void StartGameSession()
        {
            _timerStageGameSessionSystem.ShowTimer();
            _gameSessionPresenter.SetupEntityViews();
            _gameSessionModel.PlayerHero.InitBoard();
            _battlefieldSystem.Init();
            _currencyManagerSystem.Init();
    
            StartPrepareTurn();
        }

        public void CycleStartPrepareTurn()
        {
            _heroPowerSystem.ResetAllHeroPowers();
            _readyStageBackOrFightSystem.Reset();
    

            foreach (var owner in _gameSessionModel.GetAllEntityOwners())
                owner.DiscardHand();
    
            _handFillSystem.FillHandDataInDecks();
            _chakraManagerSystem.NewTurnUpdate();

            StartPrepareTurn();
        }

        private void StartPrepareTurn()
        {
            _targetingSystem.IsPreparePhase = true;
            _currencyManagerSystem.NewTurnUpdate();
            _prepareStatePresenter.StartPrepare();
            _shopSystem.UnlockShop();
            _timerStageGameSessionSystem.StartTimerPrepare(_gameSessionModel.Turn);
            _aiSystem.ExecutePreparePhase();
        }

        public void CycleStartFightTurn()
        {
            _targetingSystem.IsPreparePhase = false;
            _readyStageBackOrFightSystem.Reset();
            _shopSystem.LockShop();
            _fightStatePresenter.StartFight();
            _timerStageGameSessionSystem.StartTimerFight(_gameSessionModel.Turn);
            _aiSystem.ExecuteFightPhase();
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