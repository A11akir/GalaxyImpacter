using Feature.AI;
using Feature.UI.SelectWindowHero;

namespace Feature.GameSessionFSM
{
    public class BanStateGameSessionFSM : StateGameSessionFsm
    {
        private GameSessionFSM _gameSessionFsm;
        private SelectWindowHeroPresenter _selectWindowHeroPresenter { get; set; }
        private GameSessionData.GameSessionModel GameSessionModel { get; set; }
        private AIRandomSelectSystem _aiRandomSelectSystem { get; set; }

        public BanStateGameSessionFSM(GameSessionFSM gameSessionFsm,
            SelectWindowHeroPresenter selectWindowHeroPresenter, GameSessionData.GameSessionModel gameSessionModel,
            AIRandomSelectSystem aiRandomSelectSystem) : base(gameSessionFsm)
        {
            _selectWindowHeroPresenter = selectWindowHeroPresenter;
            _aiRandomSelectSystem = aiRandomSelectSystem;
            GameSessionModel = gameSessionModel;
            _gameSessionFsm = gameSessionFsm;
        }

        public override void Enter()
        {
            _selectWindowHeroPresenter.SetActive();
            _selectWindowHeroPresenter.SelectStartRandomHeroes();
            _selectWindowHeroPresenter.SetupRandomHeroes();
            
            if (GameSessionModel.PlayerStartGameSessionFirst()) 
                _selectWindowHeroPresenter.SetBanMode();
            else BanHeroAI();
        }

        private void BanHeroAI()
        {
            _aiRandomSelectSystem.RandomSelectValue(_selectWindowHeroPresenter._selectWindowHeroView.heroViews)
                .OnComplete(selectedHeroView =>
                {
                    _selectWindowHeroPresenter._selectWindowHeroView._selectHeroView = selectedHeroView;
                    _selectWindowHeroPresenter.BanHero();
                })
                .AIImitation();
        }

        public override void Exit()
        {
            
        }
    }
}