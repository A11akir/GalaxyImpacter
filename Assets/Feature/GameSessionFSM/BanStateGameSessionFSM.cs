using Feature.AI;
using Feature.UI.SelectWindowHero;
using UnityEngine;

namespace Feature.GameSessionFSM
{
    public class BanStateGameSessionFSM : StateGameSessionFSM
    {
        private GameSessionFSM _gameSessionFsm;
        public SelectWindowHeroPresenter _selectWindowHeroPresenter { get; set; }
        private GameSessionData.GameSessionData _gameSessionData { get; set; }

        private AIRandomSelectSystem _aiRandomSelectSystem { get; set; }

        public BanStateGameSessionFSM(GameSessionFSM gameSessionFsm,
            SelectWindowHeroPresenter selectWindowHeroPresenter, GameSessionData.GameSessionData gameSessionData,
            AIRandomSelectSystem aiRandomSelectSystem) : base(gameSessionFsm)
        {
            _gameSessionFsm = gameSessionFsm;
            _selectWindowHeroPresenter = selectWindowHeroPresenter;
            _gameSessionData = gameSessionData;
            _aiRandomSelectSystem = aiRandomSelectSystem;
        }

        public override void Enter()
        {
            _selectWindowHeroPresenter.SetActive();
            _selectWindowHeroPresenter.SelectRandomHeroes();
            _selectWindowHeroPresenter.SetRandomHeroes();
            
            if (_gameSessionData.PlayerHero.IsPlayerFirst)
            {
                _selectWindowHeroPresenter.SetBanMode();
            }
            else
            {
                _aiRandomSelectSystem.RandomSelectValue(_selectWindowHeroPresenter._selectWindowHeroView.heroViews)
                    .OnComplete(selectedHeroView =>
                    {
                        _selectWindowHeroPresenter._selectWindowHeroView._selectHeroView = selectedHeroView;
                        _selectWindowHeroPresenter.BanHero();
                        
                        _gameSessionFsm.SetState<PickStateGameSessionFSM>();
                    })
                    .AIImitation();
            }
        }

        public override void Exit()
        {
        }
    }
}