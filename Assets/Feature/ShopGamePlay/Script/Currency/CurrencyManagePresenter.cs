using Feature.GameSessionData;
using R3;

namespace Feature.ShopGamePlay.Script.Currency
{
    public class CurrencyManagePresenter : System.IDisposable
    {
        private readonly CurrencyManageView _currencyManageView;
        private readonly GameSessionModel _gameSessionData;
        private readonly CompositeDisposable _disposables = new();

        public CurrencyManagePresenter(CurrencyManageView currencyManageView, GameSessionModel gameSessionData)
        {
            _currencyManageView = currencyManageView;
            _gameSessionData = gameSessionData;
        }

        public void SubscribeToCurrencyChanges()
        {
            _gameSessionData.PlayerHero.CurrencyCount
                .Subscribe(currency => _currencyManageView.SetCurrencyText(currency))
                .AddTo(_disposables);
        }

        public void Dispose() => _disposables.Dispose();
    }
}