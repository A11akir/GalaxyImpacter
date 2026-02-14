using Feature.AI;
using Feature.GameSessionData;
using Feature.UI;
using Feature.UI.SelectWindowHero;
using UnityEngine;

namespace Feature.GameSessionFSM
{
    public class PickStateGameSessionFSM : StateGameSessionFSM
    {
        private SelectWindowHeroPresenter _selectWindowHeroPresenter { get; }
        private GameSessionPresenter _gameSessionPresenter { get; }
        private AIRandomSelectSystem _aiRandomSelectSystem { get;  }
        private GameSessionModel _gameSessionModel { get; }

        public PickStateGameSessionFSM(GameSessionFSM gameSessionFsm, SelectWindowHeroPresenter selectWindowHeroPresenter, 
            GameSessionModel gameSessionModel, AIRandomSelectSystem aiRandomSelectSystem, 
            GameSessionPresenter gameSessionPresenter) : base(gameSessionFsm)
        {
            _selectWindowHeroPresenter = selectWindowHeroPresenter;
            _aiRandomSelectSystem = aiRandomSelectSystem;
            _gameSessionPresenter = gameSessionPresenter;
            _gameSessionModel = gameSessionModel;
        }
        
        public override void Enter()
        {
            if (_gameSessionModel.PlayerStartGameSessionFirst())
                PickHeroAI();
            else
            {
                _selectWindowHeroPresenter.SetSelectMode();
                _selectWindowHeroPresenter.OnPlayerPickedHero += PickHeroAI;
            }
        }

        private void PickHeroAI()
        {
            _aiRandomSelectSystem.RandomSelectValue(_selectWindowHeroPresenter._selectWindowHeroView.heroViews)
                .OnComplete(selectedHeroView =>
                {
                    _selectWindowHeroPresenter._selectWindowHeroView._selectHeroView = selectedHeroView;
                    _selectWindowHeroPresenter.SelectHero();
                    _selectWindowHeroPresenter.ChoseSelectedHeroEnemy();
                        
                    _selectWindowHeroPresenter.OnPlayerPickedHero -= PickHeroAI;
                    _selectWindowHeroPresenter.SetSelectMode();
                })
                .AIImitation();
        }

        public override void Exit()
        {
            _selectWindowHeroPresenter.SetInactive();
            _gameSessionPresenter.SetupHeroView();
        }
    }
}