using Feature.GameSessionData;
using UnityEngine;

namespace Feature.UI
{
    public class GameSessionPresenter
    {
        private GameSessionModel _gameSessionModel;
        private GameSessionView _gameSessionView;

        public GameSessionPresenter(GameSessionView gameSessionView, GameSessionModel gameSessionModel)
        {
            _gameSessionModel = gameSessionModel;
            _gameSessionView = gameSessionView;
        }

        public void SetupHeroView()
        {
            _gameSessionView._heroView.SetViewData(_gameSessionModel.PlayerHero);
            _gameSessionView._enemyView.SetViewData(_gameSessionModel.EnemyHero);
            
            _gameSessionView._enemyView._isBlockedForSelect = true;
            _gameSessionView._heroView._isBlockedForSelect = true;
        }
    }
}