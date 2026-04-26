using Feature.StagesGameLogic;
using Feature.Timer;

namespace Feature.GameSessionFSM
{
    public class FightStateGameSessionFSM : StateGameSessionFsm
    {
        private readonly TurnCycleGameSessionSystem _turnСycleGameSessionSystem;
        private readonly TimerStageGameSessionSystem _timerStageGameSessionSystem;
        private readonly ReadyStageBackOrFightSystem _readySystem;

        public FightStateGameSessionFSM(
            GameSessionFSM gameSessionFsm,
            TurnCycleGameSessionSystem turnСycleGameSessionSystem,
            TimerStageGameSessionSystem timerStageGameSessionSystem,
            ReadyStageBackOrFightSystem readySystem) : base(gameSessionFsm)
        {
            _turnСycleGameSessionSystem = turnСycleGameSessionSystem;
            _timerStageGameSessionSystem = timerStageGameSessionSystem;
            _readySystem = readySystem;
        }

        public override void Enter()
        {
            _timerStageGameSessionSystem.OnFightTimerEnd += OnFightEnd;
            _readySystem.OnAllReady += OnFightEnd;
            _turnСycleGameSessionSystem.CycleStartFightTurn();
        }

        public override void Exit()
        {
            _timerStageGameSessionSystem.OnFightTimerEnd -= OnFightEnd;
            _readySystem.OnAllReady -= OnFightEnd;
            _turnСycleGameSessionSystem.CycleEndFightTurn();
        }

        private void OnFightEnd() => _gameSessionFSM.SetState<PrepareStateGameSessionFSM>();
    }
}