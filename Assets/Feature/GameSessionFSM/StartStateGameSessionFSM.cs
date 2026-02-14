namespace Feature.GameSessionFSM
{
    public class StartStateGameSessionFSM : StateGameSessionFSM
    {
        private GameSessionData.GameSessionModel _gameSessionModel;
        public StartStateGameSessionFSM(GameSessionFSM gameSessionFsm, GameSessionData.GameSessionModel gameSessionModel) : base(gameSessionFsm)
        {
            _gameSessionModel = gameSessionModel;
        }

        public override void Enter()
        {
            InitializeAllListCard();
            _gameSessionModel.ChooseFirstPlayer();
            CheckGameRules();
            
        }

        private void InitializeAllListCard()
        {
            
        }

        private void CheckGameRules()
        {
            if (_gameSessionModel.PlayersHaveHero())
                _gameSessionFSM.SetState<PrepareStateGameSessionFSM>();
            else
                _gameSessionFSM.SetState<BanStateGameSessionFSM>();
        }
    }
}