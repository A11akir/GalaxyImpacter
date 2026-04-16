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
        private readonly ChakraWindowView _chakraWindowView;
        private readonly HandCardPresenter _handCardPresenter;
        private readonly HandDataRepository _handDataRepository;
        private readonly CompositeDisposable _disposables = new();

        public ChakraWindowPresenter(ChakraWindowView chakraWindowView, HandCardPresenter handCardPresenter, 
            HandDataRepository handDataRepository, CardCastSystem cardCastSystem, HandViewSwitcher handViewSwitcher)
        {
            _chakraWindowView = chakraWindowView;
            _handCardPresenter = handCardPresenter;
            _handDataRepository = handDataRepository;
            _cardCastSystem = cardCastSystem;
            _handViewSwitcher = handViewSwitcher;
        }
        
        public void SubscribeToChakraChanges(CardAndHealthEntityOwnerData owner)
        {
            owner.ChakraCount
                .Subscribe(chakra =>
                {
                    var handData = _handDataRepository.GetHandData(owner);
            
                    Debug.Log(handData.Count);
                    Debug.Log(handData[0].Data.Name);
                    if (_handViewSwitcher.CurrentOwner != owner) return;
                    
                    if (handData == null) return;

                    _chakraWindowView.SetChakraText(chakra);
                    _handCardPresenter.ChakraCheckCanCastCard(handData, chakra);
                    _cardCastSystem.ChakraCheckCanCastCard(handData, chakra);
                })
                .AddTo(_disposables);
        }

        public void Dispose() => _disposables.Dispose();
    }
}
