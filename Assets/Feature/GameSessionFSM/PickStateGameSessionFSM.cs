using Feature.AI;
using Feature.GameSessionData;
using Feature.UI;
using Feature.UI.SelectWindowHero;
using UnityEngine;

namespace Feature.GameSessionFSM
{
    public class PickStateGameSessionFSM : StateGameSessionFSM
    {
        private SelectWindowHeroPresenter _selectWindowHeroPresenter { get; set; }
        private GameSessionPlayerData  _gameSessionPlayerData { get; set; }
        private GameSessionDataView  _gameSessionDataView { get; set; }
        private AIRandomSelectSystem _aiRandomSelectSystem { get; set; }
        private GameSessionFSM _gameSessionFsm { get; set; }
        private GameSessionData.GameSessionData _gameSessionData { get; set; }

        public PickStateGameSessionFSM(GameSessionFSM gameSessionFsm,
            SelectWindowHeroPresenter selectWindowHeroPresenter, GameSessionPlayerData gameSessionPlayerData, GameSessionData.GameSessionData gameSessionData, AIRandomSelectSystem aiRandomSelectSystem, GameSessionDataView gameSessionDataView) : base(gameSessionFsm)
        {
            _gameSessionFsm = gameSessionFsm;
            _selectWindowHeroPresenter = selectWindowHeroPresenter;
            _gameSessionPlayerData = gameSessionPlayerData;
            _gameSessionData = gameSessionData;
            _aiRandomSelectSystem = aiRandomSelectSystem;
            _gameSessionDataView = gameSessionDataView;
        }

  
        
        public override void Enter()
        {
            if (_gameSessionData.PlayerHero.IsPlayerFirst)
                PickHeroAI();
            else
            {
                _selectWindowHeroPresenter.SetSelectMode();
                _selectWindowHeroPresenter.OnPickedHero += PickHeroAI;
            }
        }

        public void PickHeroAI()
        {
            _aiRandomSelectSystem.RandomSelectValue(_selectWindowHeroPresenter._selectWindowHeroView.heroViews)
                .OnComplete(selectedHeroView =>
                {
                    _selectWindowHeroPresenter._selectWindowHeroView._selectHeroView = selectedHeroView;
                    _selectWindowHeroPresenter.SelectHero();
                    _selectWindowHeroPresenter.ChoseHeroEnemy();
                        
                    _selectWindowHeroPresenter.OnPickedHero -= PickHeroAI;
                    _selectWindowHeroPresenter.SetSelectMode();
                })
                .AIImitation();
        }

        public override void Exit()
        {
            _selectWindowHeroPresenter.SetInactive();

            _gameSessionDataView._heroView.SetData(_gameSessionData.PlayerHero);
            _gameSessionDataView._enemyView.SetData(_gameSessionData.EnemyHero);
            _gameSessionDataView._heroView._isBlocked = true;
            _gameSessionDataView._enemyView._isBlocked = true;
        }
    }
}