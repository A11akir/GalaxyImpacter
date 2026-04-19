// TimerStageGameSessionPresenter.cs
using Feature.GameSessionData;

namespace Feature.Timer
{
    public class TimerStageGameSessionPresenter
    {
        private readonly TimerSystem _timerSystem;
        private readonly TimerStageGameSessionSystemView _view;

        public TimerStageGameSessionPresenter(TimerSystem timerSystem, TimerStageGameSessionSystemView view)
        {
            _timerSystem = timerSystem;
            _view = view;
        }

        public void ShowTimer() => _view.gameObject.SetActive(true);
        public void Tick()
        {
            if (!_timerSystem.IsRunning) return;
            _view.SetTime(_timerSystem.TimeLeft);
        }
    }
}