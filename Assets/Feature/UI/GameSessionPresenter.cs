using Feature.GameSessionData;
using Feature.HandLogic;
using UnityEngine;

namespace Feature.UI
{
    public class GameSessionPresenter
    {
        private GameSessionModel _gameSessionModel;
        private GameSessionView _gameSessionView;
        private HandViewSwitcher _handViewSwitcher;

        public GameSessionPresenter(GameSessionView gameSessionView, GameSessionModel gameSessionModel, HandViewSwitcher handViewSwitcher)
        {
            _gameSessionModel = gameSessionModel;
            _gameSessionView = gameSessionView;
            _handViewSwitcher = handViewSwitcher;
        }

        public void SetupHeroView()
        {
            _gameSessionView._heroView.SetViewData(_gameSessionModel.PlayerHero);
            _gameSessionView._enemyView.SetViewData(_gameSessionModel.EnemyHero);
    
            _gameSessionView._enemyView._isBlockedForSelect = true;
            _gameSessionView._heroView._isBlockedForSelect = true;
    
            _gameSessionView._heroView.SetGameplayMode(true);
    
            SubscribeHeroViewClick();
        }

        private void SubscribeHeroViewClick()
        {
            var playerEntity = _gameSessionModel.PlayerHero.MainHeroEntity();
            
            _gameSessionView._heroView.OnEntityClicked += () => 
                _handViewSwitcher.SwitchTo(playerEntity);
            
            _handViewSwitcher.OnOwnerSwitched += owner =>
                _gameSessionView._heroView.SetSelected(owner == playerEntity);
        }
    }
}