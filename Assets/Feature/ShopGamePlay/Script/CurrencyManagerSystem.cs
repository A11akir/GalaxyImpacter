using Feature.GameSessionData;
using R3;

namespace Feature.ShopGamePlay.Script
{
    public class CurrencyManagerSystem
    {
        private readonly GameSessionModel _gameSessionModel;

        public CurrencyManagerSystem(GameSessionModel gameSessionModel)
        {
            _gameSessionModel = gameSessionModel;
        }

        public void NewTurnUpdate()
        {
            AddGoldHeroForNewTurn();
        }

        private void AddGoldHeroForNewTurn()
        {
            _gameSessionModel.EnemyHero.Currency += 15;
            _gameSessionModel.PlayerHero.Currency += 15;
            
            // _gameSessionModel.EnemyHero.AddCurrency(15);
            // _gameSessionModel.PlayerHero.AddCurrency(15);
        }
    }
}