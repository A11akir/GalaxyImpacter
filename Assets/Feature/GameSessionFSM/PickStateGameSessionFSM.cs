using Feature.AI;
using Feature.GameSessionData;
using Feature.UI;
using Feature.UI.SelectWindowHero;

namespace Feature.GameSessionFSM
{
    public class PickStateGameSessionFSM : StateGameSessionFsm
    {
        private SelectWindowHeroPresenter _selectWindowHeroPresenter;
        private GameSessionPresenter _gameSessionPresenter;
        private AIActionExecutor _actionExecutor;
        private GameSessionModel _gameSessionModel;

        public PickStateGameSessionFSM(
            GameSessionFSM gameSessionFsm, 
            SelectWindowHeroPresenter selectWindowHeroPresenter, 
            GameSessionModel gameSessionModel, 
            AIActionExecutor actionExecutor, // ← заменили
            GameSessionPresenter gameSessionPresenter) : base(gameSessionFsm)
        {
            _selectWindowHeroPresenter = selectWindowHeroPresenter;
            _actionExecutor = actionExecutor;
            _gameSessionPresenter = gameSessionPresenter;
            _gameSessionModel = gameSessionModel;
        }

        public override void Enter()
        {   
            if (_gameSessionModel.PlayerStartGameSessionFirst()) 
                PickHeroAI();
            else 
                PickHeroPlayer();
        }

        private void PickHeroPlayer()
        {
            _selectWindowHeroPresenter.SetSelectMode();
            _selectWindowHeroPresenter.OnPlayerPickedHero += PickHeroAI;
        }

        private void PickHeroAI()
        {
            var heroViews = _selectWindowHeroPresenter._selectWindowHeroView.heroViews;
        
            _actionExecutor.SelectAndExecute(heroViews, selectedHeroView =>
            {
                _selectWindowHeroPresenter._selectWindowHeroView._selectHeroView = selectedHeroView;
                _selectWindowHeroPresenter.SelectHero();
                _selectWindowHeroPresenter.ChoseSelectedHeroEnemy();
                _selectWindowHeroPresenter.OnPlayerPickedHero -= PickHeroAI;
                _selectWindowHeroPresenter.SetSelectMode();
            });
        }

        public override void Exit()
        {
            _selectWindowHeroPresenter.SetInactive();
            _gameSessionPresenter.SetupHeroView();
        }
    }
}