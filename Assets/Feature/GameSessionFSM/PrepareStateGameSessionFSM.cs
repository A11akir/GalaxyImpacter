
using Feature.Card.Script;
using Feature.GameSessionData;

namespace Feature.GameSessionFSM
{
    public class PrepareStateGameSessionFSM : StateGameSessionFsm
    {
        private GameSessionModel _gameSessionModel;
        private TurnСycleGameSessionSystem _turnСycleGameSessionSystem;
        
        public PrepareStateGameSessionFSM(GameSessionFSM gameSessionFsm, GameSessionModel gameSessionModel,
            TurnСycleGameSessionSystem turnСycleGameSessionSystem) : base(gameSessionFsm)
        {
            _gameSessionModel = gameSessionModel;
            _turnСycleGameSessionSystem = turnСycleGameSessionSystem;
        }

        public override void Enter()
        {
            
            if (_gameSessionModel.IsFirstTurn())
            {
                _turnСycleGameSessionSystem.StartGameSession();
                return;
            }

            
            _turnСycleGameSessionSystem.CycleTurn();
        }
        
    }
}