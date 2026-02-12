namespace Feature.GameSessionFSM
{
    public class StartStateGameSessionFSM : StateGameSessionFSM
    {
        private GameSessionData.GameSessionData _gameSessionData;
        public StartStateGameSessionFSM(GameSessionFSM gameSessionFsm) : base(gameSessionFsm)
        {
            
        }

        public override void Enter()
        {
            CheckGameRules();
            
        }

        private void CheckGameRules()
        {
            if (_gameSessionData.PlayersHaveHero())
            {
                _gameSessionFSM.SetState<PrepareStateGameSessionFSM>();
            }
            else
            {
                _gameSessionFSM.SetState<BanStateGameSessionFSM>();
            }
        }
    }
}