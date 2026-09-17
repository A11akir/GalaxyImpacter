// TimerSystem.cs
using System;

namespace Feature.Timer
{
    public class TimerSystem
    {
        private float _timeLeft;
        private float _duration;
        private bool _isRunning;

        public event Action OnTimerEnd;
        public float TimeLeft => _timeLeft;
        public float Duration => _duration;
        public float NormalizedTime => _duration > 0 ? 1f - (_timeLeft / _duration) : 0f;
        public bool IsRunning => _isRunning;

        public void Start(float duration)
        {
            _duration = duration;
            _timeLeft = duration;
            _isRunning = true;
        }

        public void Stop()
        {
            _isRunning = false;
            _timeLeft = 0;
        }

        public void Tick(float deltaTime)
        {
            if (!_isRunning) return;

            _timeLeft -= deltaTime;

            if (_timeLeft <= 0)
            {
                _timeLeft = 0;
                _isRunning = false;
                OnTimerEnd?.Invoke();
            }
        }
    }
}