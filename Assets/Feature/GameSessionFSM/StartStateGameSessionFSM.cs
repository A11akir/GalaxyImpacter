namespace Feature.GameSessionFSM
{
    public class StartStateGameSessionFSM : StateGameSessionFSM
    {
        private GameSessionData.GameSessionData _gameSessionData;
        public StartStateGameSessionFSM(GameSessionFSM gameSessionFsm, GameSessionData.GameSessionData gameSessionData) : base(gameSessionFsm)
        {
            _gameSessionData = gameSessionData;
        }

        public override void Enter()
        {
            _gameSessionData.ChooseFirstPlayer();
            CheckGameRules();
        }

        private void CheckGameRules()
        {
            if (_gameSessionData.PlayersHaveHero())
                _gameSessionFSM.SetState<PrepareStateGameSessionFSM>();
            else
                _gameSessionFSM.SetState<BanStateGameSessionFSM>();
        }
    }
}