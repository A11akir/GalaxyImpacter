using Feature.GameSessionData;
using UnityEngine;
using Zenject;

namespace Feature.Card.Script
{
    public class CardCastSystem
    {
        private GameSessionModel _gameSessionModel;
        private HandCardPresenter _handCardPresenter;
        private IInstantiator _instantiator; // Используем IInstantiator

        public CardCastSystem(
            GameSessionModel gameSessionModel, 
            HandCardPresenter handCardPresenter,
            IInstantiator instantiator) // Инжектим IInstantiator
        {
            _gameSessionModel = gameSessionModel;
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
                        break;
                        
                    default:
                        _instantiator.InstantiateComponent<NonTargetCardUseBehaviour>(cardObject);
                        break;
                }
            }
        }
    }
}