using Feature.GameSessionData;
using UnityEngine;
using Zenject;

namespace Feature.Card.Script
{
    public class CardCastSystem
    {
        private HandCardPresenter _handCardPresenter;
        private IInstantiator _instantiator;
        public CardCastSystem(HandCardPresenter handCardPresenter, IInstantiator instantiator)
        {
            _handCardPresenter = handCardPresenter;
            _instantiator = instantiator;
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
                        var selectObject = _instantiator.InstantiateComponent<SelectTargetCardUseBehaviour>(cardObject);

                        selectObject.cardObject = pair.view._cardContainer;
                        selectObject.cursorArrowHead = pair.view._cursorArrowHead;
                        selectObject.cursorArrowLine = pair.view._cursorArrowLine;
                        selectObject.Init();
                        break;
                        
                    default:
                        _instantiator.InstantiateComponent<NonTargetCardUseBehaviour>(cardObject);
                        break;
                }
            }
        }
    }
}