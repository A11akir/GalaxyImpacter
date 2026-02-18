using Feature.Card.Script;
using Feature.GameSessionData;
using R3;


namespace Feature.Chakra
{
    public class ChakraWindowPresenter 
    {
        private readonly ChakraWindowView _chakraWindowView;
        private CardCastSystem  _cardCastSystem;
        private readonly GameSessionModel _gameSessionData;
        private readonly CompositeDisposable _disposables = new();

        public ChakraWindowPresenter(ChakraWindowView chakraWindowView, GameSessionModel gameSessionData, CardCastSystem cardCastSystem)
        {
            _chakraWindowView = chakraWindowView;
            _gameSessionData = gameSessionData;
            _cardCastSystem = cardCastSystem;
        }

        public void SubscribeToChakraChanges()
        {
            _gameSessionData.PlayerHero.ChakraCount
                .Subscribe(currency => _chakraWindowView.SetChakraText(currency))
                .AddTo(_disposables);
            _gameSessionData.PlayerHero.ChakraCount
                .Subscribe(_ => _cardCastSystem.ManaCheckCanCastCard())
                .AddTo(_disposables);
        }

        public void Dispose() => _disposables.Dispose();
    }
}
