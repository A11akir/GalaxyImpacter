using Feature.Card.Script;
using Feature.GameSessionData;
using R3;


namespace Feature.Chakra
{
    public class ChakraWindowPresenter 
    {
        private readonly CardCastSystem _cardCastSystem;
        
        private readonly ChakraWindowView _chakraWindowView;
        private readonly HandCardPresenter _handCardPresenter;
        private readonly GameSessionModel _gameSessionData;
        private readonly HandDataRepository _handDataRepository;
        private readonly CompositeDisposable _disposables = new();

        public ChakraWindowPresenter(ChakraWindowView chakraWindowView, GameSessionModel gameSessionData, HandCardPresenter handCardPresenter, HandDataRepository handDataRepository, CardCastSystem cardCastSystem)
        {
            _chakraWindowView = chakraWindowView;
            _gameSessionData = gameSessionData;
            _handCardPresenter = handCardPresenter;
            _handDataRepository = handDataRepository;
            _cardCastSystem = cardCastSystem;
        }

        public void SubscribeToChakraChanges()
        {
            _gameSessionData.PlayerHero.ChakraCount
                .Subscribe(currency => _chakraWindowView.SetChakraText(currency))
                .AddTo(_disposables);
            _gameSessionData.PlayerHero.ChakraCount
                .Subscribe(_ => _handCardPresenter.ChakraCheckCanCastCard(_handDataRepository._handData))
                .AddTo(_disposables);          _gameSessionData.PlayerHero.ChakraCount
                .Subscribe(_ => _cardCastSystem.ChakraCheckCanCastCard(_handDataRepository._handData))
                .AddTo(_disposables);
        }

        public void Dispose() => _disposables.Dispose();
    }
}
