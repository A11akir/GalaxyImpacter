namespace Feature.StagesGameLogic
{
    public class PrepareStatePresenter
    {
        private readonly PrepareStateView _prepareStateView;
        private readonly ReadyStageBackOrFightSystem _readySystem;

        public PrepareStatePresenter(PrepareStateView prepareStateView, ReadyStageBackOrFightSystem readySystem)
        {
            _prepareStateView = prepareStateView;
            _readySystem = readySystem;
        }

        public void StartPrepare()
        {
            _prepareStateView.StartPrepare();
            _prepareStateView.OnReadyClicked += _readySystem.SetPlayerReady;
        }

        public void EndPrepare()
        {
            _prepareStateView.EndPrepare();
            _prepareStateView.OnReadyClicked -= _readySystem.SetPlayerReady;
        }
    }
}