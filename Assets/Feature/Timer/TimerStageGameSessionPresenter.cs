// TimerStageGameSessionPresenter.cs

using Shaders;
using UnityEngine;

namespace Feature.Timer
{
    public class TimerStageGameSessionPresenter
    {
        private readonly TimerSystem _timerSystem;
        private readonly TimerStageGameSessionSystemView _view;
        private readonly TimeClockView _timeClock;

        private const float FadeOutDuration = 5f;

        public TimerStageGameSessionPresenter(
            TimerSystem timerSystem,
            TimerStageGameSessionSystemView view,
            TimeClockView timeClock)
        {
            _timerSystem = timerSystem;
            _view = view;
            _timeClock = timeClock;
        }

        public void ShowTimer() => _view.gameObject.SetActive(true);

        public void Tick()
        {
            if (!_timerSystem.IsRunning) return;

            _view.SetTime(_timerSystem.TimeLeft);
            _timeClock.SetFillAmount(_timerSystem.NormalizedTime);

            float t = Mathf.Clamp01(_timerSystem.TimeLeft / FadeOutDuration);

            // зона затухания сужается к 0
            float fadeLength = Mathf.Lerp(0.01f, 0.3f, t);
            _timeClock.SetAlphaFadeLength(fadeLength);
        }
    }
}