using System;
using Feature.Battlefield.Script.View;

namespace Feature.StagesGameLogic
{
    public class PrepareStatePresenter
    {
        private readonly PrepareStateView _prepareStateView;
        private readonly ReadyStageBackOrFightSystem _readySystem;
        private readonly WarFogView _warFogView;
        private Action _onReadyClicked;

        public PrepareStatePresenter(PrepareStateView prepareStateView, ReadyStageBackOrFightSystem readySystem, WarFogView warFogView)
        {
            _prepareStateView = prepareStateView;
            _readySystem = readySystem;
            _warFogView = warFogView;

            _onReadyClicked += () =>
            {
                _prepareStateView.SetReadyButtonInteractable(false);
                _readySystem.SetPlayerReady();
            };
        }

        public void StartPrepare()
        {
            _prepareStateView.StartPrepare(); 
            _prepareStateView.OnReadyClicked += _onReadyClicked;
            _warFogView.ShowFog();
        }

        public void EndPrepare()
        {
            _prepareStateView.EndPrepare();
            _prepareStateView.OnReadyClicked -= _onReadyClicked;
        }
    }
}