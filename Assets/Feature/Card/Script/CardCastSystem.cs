using Feature.GameSessionData;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Feature.Card.Script
{
    public class CardCastSystem
    {
        private IInstantiator _instantiator;
        private TransformCardHandLogic _cardHandLogic;
        private ITargetCardBehaviour _targetCardBehaviour;
        private GameSessionModel _gameSessionModel;

        public CardCastSystem(IInstantiator instantiator, GameSessionModel gameSessionModel, TransformCardHandLogic cardHandLogic, ITargetCardBehaviour targetCardBehaviour)
        {
            _instantiator = instantiator;
            _gameSessionModel = gameSessionModel;
            _cardHandLogic = cardHandLogic;
            _targetCardBehaviour = targetCardBehaviour;
        }

        public void AddBehavioursToCards(List<HandCardData> handData)
        {
            foreach (var cardData in handData)
            {
                switch (cardData.Data.targetSpellType)
                {
                    case TargetSpellType.AnyTarget:
                        var selectObjectTarget = _instantiator.InstantiateComponent<SelectTargetCardUseBehaviour>(cardData.View.gameObject);
                        
                        selectObjectTarget.cardObject = cardData.View._cardContainer;
                        selectObjectTarget.cursorArrowHead = cardData.View._cursorArrowHead;
                        selectObjectTarget.cursorArrowLine = cardData.View._cursorArrowLine;
                        selectObjectTarget.Init();
                        cardData.Behaviour = selectObjectTarget;
                        break;
                        
                    default:
                        var nonTargetBehaviour = _instantiator.InstantiateComponent<NonTargetCardUseBehaviour>(cardData.View.gameObject);
                        cardData.Behaviour = nonTargetBehaviour;
                        break;
                }
            }
        }



        public void ChakraCheckCanCastCard(List<HandCardData> handData)
        {
            foreach (var cardData in handData)
            {
                /*cardData.Behaviour.CanCastCard(_gameSessionModel.PlayerHero.Chakra > cardData.Data.Cost);*/
            }
        }
    }
}