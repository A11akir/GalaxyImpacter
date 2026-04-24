using System;

namespace Feature.EndGameSession
{
    public class GameOverSystem
    {
        public event Action<bool> OnGameOver;

        public void HandleGameOver(bool isPlayer) => OnGameOver?.Invoke(isPlayer);
    }
}