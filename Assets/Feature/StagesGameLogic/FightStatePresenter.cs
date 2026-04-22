using System;

namespace Feature.StagesGameLogic
{
    public class FightStatePresenter
    {
        private readonly FightStateView _fightStateView;
        private readonly ReadyStageBackOrFightSystem _readySystem;
        private Action _onReadyClicked;

        public FightStatePresenter(FightStateView fightStateView, ReadyStageBackOrFightSystem readySystem)
        {
            _fightStateView = fightStateView;
            _readySystem = readySystem;
        
            _onReadyClicked = () =>
            {
                _fightStateView.SetReadyButtonInteractable(false);
                _readySystem.SetPlayerReady();
            };
        }

        public void StartFight()
        {
            _fightStateView.StartFight();
            _fightStateView.OnReadyClicked += _onReadyClicked;
        }

        public void EndFight()
        {
            _fightStateView.EndFight();
            _fightStateView.OnReadyClicked -= _onReadyClicked;
        }
    }
}