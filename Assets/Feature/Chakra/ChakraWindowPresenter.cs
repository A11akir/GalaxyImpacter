using Feature.Card.Script;
using Feature.GameSessionData;
using Feature.HandLogic;
using R3;
using UnityEngine;

namespace Feature.Chakra
{
    public class ChakraWindowPresenter 
    {
        private readonly CardCastSystem _cardCastSystem;
        private readonly HandViewSwitcher _handViewSwitcher; 
        private readonly HandCardPresenter _handCardPresenter;
        private readonly HandDataRepository _handDataRepository;
        private readonly CompositeDisposable _disposables = new();

        public ChakraWindowPresenter(HandCardPresenter handCardPresenter, 
            HandDataRepository handDataRepository, CardCastSystem cardCastSystem, HandViewSwitcher handViewSwitcher)
        {
            _handCardPresenter = handCardPresenter;
            _handDataRepository = handDataRepository;
            _cardCastSystem = cardCastSystem;
            _handViewSwitcher = handViewSwitcher;

            _handViewSwitcher.OnOwnerSwitched += OnOwnerSwitched;
        }

        private void OnOwnerSwitched(CardAndHealthEntityOwnerData owner)
        {
            var container = _handViewSwitcher.GetContainer(owner);
            if (container == null) return;

            var handData = _handDataRepository.GetHandData(owner);
            if (handData == null) return;

            container.ChakraWindowView.SetChakraText(owner.Chakra);
            _handCardPresenter.ChakraCheckCanCastCard(handData, owner.Chakra);
            _cardCastSystem.ChakraCheckCanCastCard(handData, owner.Chakra);
        }

        public void SubscribeToChakraChanges(CardAndHealthEntityOwnerData owner, ChakraWindowView chakraWindowView)
        {
            owner.ChakraCount
                .Subscribe(chakra =>
                {
                    if (_handViewSwitcher.CurrentOwner != owner) return;

                    var handData = _handDataRepository.GetHandData(owner);
                    if (handData == null) return;

                    chakraWindowView.SetChakraText(chakra);
                    _handCardPresenter.ChakraCheckCanCastCard(handData, chakra);
                    _cardCastSystem.ChakraCheckCanCastCard(handData, chakra);
                })
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _handViewSwitcher.OnOwnerSwitched -= OnOwnerSwitched;
            _disposables.Dispose();
        }
    }
}