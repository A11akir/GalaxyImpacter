namespace Feature.EndGameSession
{
    public class GameOverPresenter
    {
        private readonly GameOverSystem _gameOverSystem;
        private readonly GameOverView _gameOverView;

        public GameOverPresenter(GameOverSystem gameOverSystem, GameOverView gameOverView)
        {
            _gameOverSystem = gameOverSystem;
            _gameOverView = gameOverView;

            _gameOverSystem.OnGameOver += OnGameOver;
        }

        private void OnGameOver(bool isPlayer)
        {
            if (isPlayer)
                _gameOverView.ShowDefeat();
            else
                _gameOverView.ShowVictory();
        }
    }
}