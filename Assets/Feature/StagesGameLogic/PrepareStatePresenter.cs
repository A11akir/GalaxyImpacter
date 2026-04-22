using System;

namespace Feature.StagesGameLogic
{
    public class PrepareStatePresenter
    {
        private readonly PrepareStateView _prepareStateView;
        private readonly ReadyStageBackOrFightSystem _readySystem;
        private Action _onReadyClicked;

        public PrepareStatePresenter(PrepareStateView prepareStateView, ReadyStageBackOrFightSystem readySystem)
        {
            _prepareStateView = prepareStateView;
            _readySystem = readySystem;
            
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

        }

        public void EndPrepare()
        {
            _prepareStateView.EndPrepare();
            _prepareStateView.OnReadyClicked -= _onReadyClicked;
        }
    }
}