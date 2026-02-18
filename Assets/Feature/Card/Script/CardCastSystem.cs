using Feature.GameSessionData;
using UnityEngine;
using Zenject;

namespace Feature.Card.Script
{
    public class CardCastSystem
    {
        private HandCardPresenter _handCardPresenter;
        private IInstantiator _instantiator;
        private readonly GameSessionModel _gameSessionModel;

        public CardCastSystem(HandCardPresenter handCardPresenter, IInstantiator instantiator, GameSessionModel gameSessionModel)
        {
            _handCardPresenter = handCardPresenter;
            _instantiator = instantiator;
            _gameSessionModel = gameSessionModel;
        }

        public void ManaCheckCanCastCard()
        {
            foreach (var card in _handCardPresenter._handCards)
            {
                card.view.SetCanCastView(_gameSessionModel.PlayerHero.Chakra >= card.data.Cost);
                card.
            }
        }
        
        public void InitPropertyCard()
        {
            foreach (var pair in _handCardPresenter._handCards)
            {
                CardStatsData data = pair.data;
                GameObject cardObject = pair.view.gameObject;
        
                switch (data.targetSpellType)
                {
                    case TargetSpellType.AnyTarget:
                        var selectObjectTarget = _instantiator.InstantiateComponent<SelectTargetCardUseBehaviour>(cardObject);

                        selectObjectTarget.cardObject = pair.view._cardContainer;
                        selectObjectTarget.cursorArrowHead = pair.view._cursorArrowHead;
                        selectObjectTarget.cursorArrowLine = pair.view._cursorArrowLine;
                        selectObjectTarget.Init();
                        break;
                        
                    default:
                        _instantiator.InstantiateComponent<NonTargetCardUseBehaviour>(cardObject);
                        break;
                }
                
            }
        }
        
        public void CheckCastCardData()
        {
            
        }

        public void CastCard()
        {
            
        }
    }
}