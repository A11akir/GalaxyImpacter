using Feature.Card.Script;
using Feature.GameSessionData;
using R3;
using UnityEngine;


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

        public void SubscribeToChakraChanges(CardAndHealthEntityOwnerData owner)
        {
            owner.ChakraCount
                .Subscribe(chakra =>
                {
                        _chakraWindowView.SetChakraText(chakra);

                    var handData = _handDataRepository.GetHandData(owner);
                    if (handData == null) return;

                    _handCardPresenter.ChakraCheckCanCastCard(handData, chakra);
                    _cardCastSystem.ChakraCheckCanCastCard(handData, chakra);
                })
                .AddTo(_disposables);
        }

        public void Dispose() => _disposables.Dispose();
    }
}
