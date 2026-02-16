using Feature.GameSessionData;
using R3;


namespace Feature.Chakra
{
    public class ChakraWindowPresenter 
    {
        private readonly ChakraWindowView _chakraWindowView;
        private readonly GameSessionModel _gameSessionData;
        private readonly CompositeDisposable _disposables = new();

        public ChakraWindowPresenter(ChakraWindowView chakraWindowView, GameSessionModel gameSessionData)
        {
            _chakraWindowView = chakraWindowView;
            _gameSessionData = gameSessionData;
        }

        public void SubscribeToChakraChanges()
        {
            _gameSessionData.PlayerHero.ChakraCount
                .Subscribe(currency => _chakraWindowView.SetChakraText(currency))
                .AddTo(_disposables);
        }

        public void Dispose() => _disposables.Dispose();
    }
}
