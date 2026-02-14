using Feature.GameSessionData;

namespace Feature.ShopGamePlay.Script.Currency
{
    public class CurrencyManagerSystem
    {
        private readonly GameSessionModel _gameSessionModel;
        private readonly CurrencyManagePresenter _currencyManagePresenter;

        public CurrencyManagerSystem(GameSessionModel gameSessionModel, CurrencyManagePresenter currencyManagePresenter)
        {
            _gameSessionModel = gameSessionModel;
            _currencyManagePresenter = currencyManagePresenter;
        }

        public void Init()
        {
            _currencyManagePresenter.SubscribeToCurrencyChanges();
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