using Feature.Card.Script;
using Feature.GameSessionData;
using Feature.HandLogic;
using Feature.Hero;
using Feature.Hero.Script;
using R3;

namespace Feature.Chakra
{
    public class ChakraWindowPresenter
    {
        private readonly HandCardCastabilitySystem _castabilitySystem;
        private readonly HandViewSwitcher _handViewSwitcher;
        private readonly HandDataRepository _handDataRepository;
        private readonly CompositeDisposable _disposables = new();
        private readonly HeroPowerPresenter _heroPowerPresenter;

        public ChakraWindowPresenter(
            HandCardCastabilitySystem castabilitySystem,
            HandDataRepository handDataRepository,
            HandViewSwitcher handViewSwitcher,
            HeroPowerPresenter heroPowerPresenter)
        {
            _castabilitySystem = castabilitySystem;
            _handDataRepository = handDataRepository;
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
            _castabilitySystem.RefreshHand(handData, owner.Chakra); // ← один вызов вместо двух
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
                    _castabilitySystem.RefreshHand(handData, chakra); // ← один вызов вместо двух
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