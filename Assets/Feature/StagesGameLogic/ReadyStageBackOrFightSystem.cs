using System;

namespace Feature.StagesGameLogic
{
    public class ReadyStageBackOrFightSystem
    {
        private bool _playerReady;
        private bool _enemyReady;

        public event Action OnAllReady;

        public void SetPlayerReady()
        {
            _playerReady = true;
            CheckAllReady();
        }

        public void SetEnemyReady()
        {
            _enemyReady = true;
            CheckAllReady();
        }

        public void Reset()
        {
            _playerReady = false;
            _enemyReady = false;
        }

        private void CheckAllReady()
        {
            if (_playerReady && _enemyReady)
                OnAllReady?.Invoke();
        }
    }
}