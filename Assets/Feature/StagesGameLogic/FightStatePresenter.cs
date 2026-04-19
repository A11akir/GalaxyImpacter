namespace Feature.StagesGameLogic
{
    public class FightStatePresenter
    {
        private readonly FightStateView _fightStateView;
        private readonly ReadyStageBackOrFightSystem _readySystem;

        public FightStatePresenter(FightStateView fightStateView, ReadyStageBackOrFightSystem readySystem)
        {
            _fightStateView = fightStateView;
            _readySystem = readySystem;
        }
    
        public void StartFight()
        {
            _fightStateView.StartFight();
            _fightStateView.OnReadyClicked += _readySystem.SetPlayerReady;
        }

        public void EndFight()
        {
            _fightStateView.EndFight();
            _fightStateView.OnReadyClicked -= _readySystem.SetPlayerReady;
        }
    }
}