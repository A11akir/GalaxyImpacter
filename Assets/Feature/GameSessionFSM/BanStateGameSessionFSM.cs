using Feature.UI.SelectWindowHero;

namespace Feature.GameSessionFSM
{
    public class BanStateGameSessionFSM : StateGameSessionFSM
    {
        private SelectWindowHeroPresenter _selectWindowHeroPresenter { get; set; }
        
        public BanStateGameSessionFSM(GameSessionFSM gameSessionFsm, SelectWindowHeroPresenter selectWindowHeroPresenter) : base(gameSessionFsm)
        {
            _selectWindowHeroPresenter = selectWindowHeroPresenter;
        }

        public override void Enter()
        {
            _selectWindowHeroPresenter.SetActive();
            _selectWindowHeroPresenter.SelectRandomHeroes();
            _selectWindowHeroPresenter.SetRandomHeroes();
            _selectWindowHeroPresenter.SetBanMode();
            
        }
        public override void Exit()
        {
            
        }
    }
}