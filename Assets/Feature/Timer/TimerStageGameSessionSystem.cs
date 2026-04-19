using System;
using Feature.GameSessionData;

namespace Feature.Timer
{
    public class TimerStageGameSessionSystem
    {
        private readonly TimerSystem _timerSystem;
        private readonly GameSessionModel _gameSessionModel;

        public event Action OnPrepareTimerEnd;
        public event Action OnFightTimerEnd;

        private bool _isFightPhase;

        public TimerStageGameSessionSystem(TimerSystem timerSystem, GameSessionModel gameSessionModel)
        {
            _timerSystem = timerSystem;
            _gameSessionModel = gameSessionModel;
            _timerSystem.OnTimerEnd += OnTimerEnd;
        }

        public void StartTimerPrepare(int turn)
        {
            _isFightPhase = false;
            float duration = _gameSessionModel.PrepareStartTime + 5f * turn;
            _timerSystem.Start(duration);
        }

        public void StartTimerFight(int turn)
        {
            _isFightPhase = true;
            float duration = _gameSessionModel.FightStartTime + 5f * turn;
            _timerSystem.Start(duration);
        }

        private void OnTimerEnd()
        {
            if (_isFightPhase)
                OnFightTimerEnd?.Invoke();
            else
                OnPrepareTimerEnd?.Invoke();
        }

        public void Stop() => _timerSystem.Stop();
    }
}