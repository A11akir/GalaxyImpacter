using System.Collections.Generic;
using Feature.Card.Script;
using Feature.GameSessionData;
using Feature.HandLogic;
using Feature.Hero;
using UnityEngine;

namespace Feature.UI
{
    public class GameSessionPresenter
    {
        private GameSessionModel _gameSessionModel;
        private GameSessionView _gameSessionView;
        private HandViewSwitcher _handViewSwitcher;
        private TargetingSystem _targetingSystem;
        private CreateOwnerCardAndHealthEntitySystem _createOwnerCardAndHealthEntitySystem;

        public GameSessionPresenter(GameSessionView gameSessionView, GameSessionModel gameSessionModel, HandViewSwitcher handViewSwitcher, CreateOwnerCardAndHealthEntitySystem createOwnerCardAndHealthEntitySystem, TargetingSystem targetingSystem)
        {
            _gameSessionModel = gameSessionModel;
            _gameSessionView = gameSessionView;
            _handViewSwitcher = handViewSwitcher;
            _createOwnerCardAndHealthEntitySystem = createOwnerCardAndHealthEntitySystem;
            _targetingSystem = targetingSystem;
        }

        public void SetupHeroView()
        {
            _gameSessionView._heroView.SetViewData(_gameSessionModel.PlayerHero);
            _gameSessionView._enemyView.SetViewData(_gameSessionModel.EnemyHero);

            _gameSessionView._enemyView._isBlockedForSelect = true;
            _gameSessionView._heroView._isBlockedForSelect = true;

            _gameSessionView._heroView.SetGameplayMode(true);
            
            _targetingSystem.RegisterTarget(_gameSessionView._heroView.gameObject,_gameSessionModel.PlayerHero.MainHeroEntity());
            _targetingSystem.RegisterTarget(_gameSessionView._enemyView.gameObject,_gameSessionModel.EnemyHero.MainHeroEntity());

            SubscribeHeroViewClick();
        }

        public void SetupEntityViews()
        {
            _createOwnerCardAndHealthEntitySystem.CreatePlayersEntity(
                _gameSessionView._heroView,
                _gameSessionView._enemyView
            );
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