using Feature.Battlefield.Script;
using Feature.GameSessionData;
using Feature.ShopGamePlay.Script.Currency;
using Feature.Timer;
using Feature.UI;

namespace Feature.StagesGameLogic
{
    public class TurnCycleGameSessionSystem
    {
        private readonly StageManagerSystem _stageManagerSystem;
        private readonly TurnResourceManager _resourceManager;
        private readonly ReadyStageBackOrFightSystem _readySystem;
        private readonly GameSessionModel _gameSessionModel;
        private readonly GameSessionPresenter _gameSessionPresenter;
        private readonly BattlefieldSystem _battlefieldSystem;
        private readonly CurrencyManagerSystem _currencyManager;
        private readonly TimerStageGameSessionSystem _timerSystem;

        public TurnCycleGameSessionSystem(StageManagerSystem stageManagerSystem, TurnResourceManager resourceManager, GameSessionModel gameSessionModel, GameSessionPresenter gameSessionPresenter, BattlefieldSystem battlefieldSystem, CurrencyManagerSystem currencyManager, TimerStageGameSessionSystem timerSystem, ReadyStageBackOrFightSystem readySystem)
        {
            _stageManagerSystem = stageManagerSystem;
            _resourceManager = resourceManager;
            _gameSessionModel = gameSessionModel;
            _gameSessionPresenter = gameSessionPresenter;
            _battlefieldSystem = battlefieldSystem;
            _currencyManager = currencyManager;
            _timerSystem = timerSystem;
            _readySystem = readySystem;
        }


        public void StartGameSession()
        {
            _timerSystem.ShowTimer();
            _gameSessionPresenter.SetupEntityViews();
            _gameSessionModel.PlayerHero.InitBoard();
            _battlefieldSystem.Init();
            _currencyManager.Init();
        
            _stageManagerSystem.StartPreparePhase(_gameSessionModel.Turn);
        }

        public void CycleStartPrepareTurn()
        {
            _readySystem.Reset();
            _resourceManager.StartNewTurn();
            _stageManagerSystem.StartPreparePhase(_gameSessionModel.Turn);
        }

        public void CycleStartFightTurn()
        {
            _readySystem.Reset();
            _stageManagerSystem.StartFightPhase(_gameSessionModel.Turn);
        }

        public void CycleEndFightTurn() => _stageManagerSystem.EndFightPhase();
        public void CycleEndPrepareTurn() => _stageManagerSystem.EndPreparePhase();
    }
}