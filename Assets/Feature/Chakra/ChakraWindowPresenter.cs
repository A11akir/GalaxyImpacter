using Feature.Card.Script;
using Feature.GameSessionData;
using Feature.HandLogic;
using Feature.Hero;
using R3;
using UnityEngine;

namespace Feature.Chakra
{
    public class ChakraWindowPresenter 
    {
        private readonly FactoryHandBehaviourTransformCastSystem _factoryHandBehaviourTransformCastSystem;
        private readonly HandViewSwitcher _handViewSwitcher; 
        private readonly HandCardPresenter _handCardPresenter;
        private readonly HandDataRepository _handDataRepository;
        private readonly CompositeDisposable _disposables = new();
        private readonly HeroPowerPresenter _heroPowerPresenter;

        public ChakraWindowPresenter(HandCardPresenter handCardPresenter, 
            HandDataRepository handDataRepository, FactoryHandBehaviourTransformCastSystem factoryHandBehaviourTransformCastSystem, HandViewSwitcher handViewSwitcher, HeroPowerPresenter heroPowerPresenter)
        {
            _handCardPresenter = handCardPresenter;
            _handDataRepository = handDataRepository;
            _factoryHandBehaviourTransformCastSystem = factoryHandBehaviourTransformCastSystem;
            _handViewSwitcher = handViewSwitcher;
            _heroPowerPresenter = heroPowerPresenter;

            _handViewSwitcher.OnOwnerSwitched += OnOwnerSwitched;
        }

        private void OnOwnerSwitched(CardAndHealthEntityOwnerData owner)
        {
            var container = _handViewSwitcher.GetContainer(owner);
            if (!container) return;

            var handData = _handDataRepository.GetHandData(owner);
            if (handData == null) return;

            container.ChakraWindowView.SetChakraText(owner.Chakra);
            _handCardPresenter.ChakraCheckCanCastHand(handData, owner.Chakra);
            _factoryHandBehaviourTransformCastSystem.ChakraCheckCanCastCard(handData, owner.Chakra);
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
                    _handCardPresenter.ChakraCheckCanCastHand(handData, chakra);
                    _factoryHandBehaviourTransformCastSystem.ChakraCheckCanCastCard(handData, chakra);
                    _heroPowerPresenter.UpdateCanCastView();
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