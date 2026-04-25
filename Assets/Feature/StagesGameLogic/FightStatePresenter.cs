using System;
using Feature.Battlefield.Script.View;

namespace Feature.StagesGameLogic
{
    public class FightStatePresenter
    {
        private readonly FightStateView _fightStateView;
        private readonly ReadyStageBackOrFightSystem _readySystem;
        private readonly WarFogView _warFogView;
        private Action _onReadyClicked;

        public FightStatePresenter(FightStateView fightStateView, ReadyStageBackOrFightSystem readySystem, WarFogView warFogView)
        {
            _fightStateView = fightStateView;
            _readySystem = readySystem;
            _warFogView = warFogView;

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
            _warFogView.HideFog();
        }

        public void EndFight()
        {
            _fightStateView.EndFight();
            _fightStateView.OnReadyClicked -= _onReadyClicked;
        }
    }
}