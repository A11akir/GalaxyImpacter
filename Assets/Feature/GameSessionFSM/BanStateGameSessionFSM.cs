using Feature.AI;
using Feature.UI.SelectWindowHero;

namespace Feature.GameSessionFSM
{
    public class BanStateGameSessionFSM : StateGameSessionFsm
    {
        private GameSessionFSM _gameSessionFsm;
        private SelectWindowHeroPresenter _selectWindowHeroPresenter;
        private GameSessionData.GameSessionModel _gameSessionModel;
        private AIActionExecutor _actionExecutor;

        public BanStateGameSessionFSM(
            GameSessionFSM gameSessionFsm,
            SelectWindowHeroPresenter selectWindowHeroPresenter, 
            GameSessionData.GameSessionModel gameSessionModel,
            AIActionExecutor actionExecutor) : base(gameSessionFsm)
        {
            _selectWindowHeroPresenter = selectWindowHeroPresenter;
            _actionExecutor = actionExecutor;
            _gameSessionModel = gameSessionModel;
            _gameSessionFsm = gameSessionFsm;
        }

        public override void Enter()
        {
            _selectWindowHeroPresenter.SetActive();
            _selectWindowHeroPresenter.SelectStartRandomHeroes();
            _selectWindowHeroPresenter.SetupRandomHeroes();
            
            if (_gameSessionModel.PlayerStartGameSessionFirst()) 
                _selectWindowHeroPresenter.SetBanMode();
            else 
                BanHeroAI();
        }

        private void BanHeroAI()
        {
            var heroViews = _selectWindowHeroPresenter._selectWindowHeroView.heroViews;
            
            _actionExecutor.SelectAndExecute(heroViews, selectedHeroView =>
            {
                _selectWindowHeroPresenter._selectWindowHeroView._selectHeroView = selectedHeroView;
                _selectWindowHeroPresenter.BanHero();
            });
        }

        public override void Exit()
        {
            
        }
    }
}