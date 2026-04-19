using Feature.Card.Script;
using Feature.GameSessionData;
using Feature.StagesGameLogic;
using Feature.Timer;

namespace Feature.GameSessionFSM
{
    public class PrepareStateGameSessionFSM : StateGameSessionFsm
    {
        private readonly GameSessionModel _gameSessionModel;
        private readonly TurnСycleGameSessionSystem _turnСycleGameSessionSystem;
        private readonly TimerStageGameSessionSystem _timerStageGameSessionSystem;
        private readonly ReadyStageBackOrFightSystem _readySystem;

        public PrepareStateGameSessionFSM(
            GameSessionFSM gameSessionFsm,
            GameSessionModel gameSessionModel,
            TurnСycleGameSessionSystem turnСycleGameSessionSystem,
            TimerStageGameSessionSystem timerStageGameSessionSystem,
            ReadyStageBackOrFightSystem readySystem) : base(gameSessionFsm)
        {
            _gameSessionModel = gameSessionModel;
            _turnСycleGameSessionSystem = turnСycleGameSessionSystem;
            _timerStageGameSessionSystem = timerStageGameSessionSystem;
            _readySystem = readySystem;
        }

        public override void Enter()
        {
            _timerStageGameSessionSystem.OnPrepareTimerEnd += OnReadyToFight;
            _readySystem.OnAllReady += OnReadyToFight;

            _gameSessionModel.Turn++;
            
            if (_gameSessionModel.IsFirstTurn())
            {
                _turnСycleGameSessionSystem.StartGameSession();
                return;
            }

            
            _turnСycleGameSessionSystem.CycleStartPrepareTurn();
        }

        public override void Exit()
        {
            _timerStageGameSessionSystem.OnPrepareTimerEnd -= OnReadyToFight;
            _readySystem.OnAllReady -= OnReadyToFight;
            _turnСycleGameSessionSystem.CycleEndPrepareTurn();
        }

        private void OnReadyToFight() => _gameSessionFSM.SetState<FightStateGameSessionFSM>();
    }
}