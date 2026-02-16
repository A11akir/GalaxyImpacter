using Feature.GameSessionData;
using UnityEngine;

namespace Feature.Card.Script
{
    public class CardCastSystem
    {
        private GameSessionModel _gameSessionModel;
        private HandCardPresenter _handCardPresenter;

        public CardCastSystem(GameSessionModel gameSessionModel, HandCardPresenter handCardPresenter)
        {
            _gameSessionModel = gameSessionModel;
            _handCardPresenter = handCardPresenter;
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
                        var selectObject =  cardObject.AddComponent<SelectTargetCardUseBehaviour>();

                        selectObject.cardObject = pair.view._cardContainer;
                        selectObject.cursorArrowHead = pair.view._cursorArrowHead;
                        selectObject.cursorArrowLine = pair.view._cursorArrowLine;
                        break;
                    default:
                        cardObject.AddComponent<NonTargetCardUseBehaviour>();
                        break;
                }
            }
        }
    }
}