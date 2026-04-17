using Feature.GameSessionData;

namespace Feature.Health
{
    public class HealthManagerSystem
    {
        private readonly GameSessionModel _gameSessionModel;

        public HealthManagerSystem(GameSessionModel gameSessionModel)
        {
            _gameSessionModel = gameSessionModel;
        }
        
        public void Init(CardAndHealthEntityOwnerData owner)
        {
            
        }
    }
}