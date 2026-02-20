using Feature.GameSessionData;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Feature.Card.Script
{
    public class CardCastSystem
    {
        private IInstantiator _instantiator;
        private GameSessionModel _gameSessionModel;
        
        public CardCastSystem(IInstantiator instantiator, GameSessionModel gameSessionModel)
        {
            _instantiator = instantiator;
            _gameSessionModel = gameSessionModel;
        }

        public void AddBehavioursToCards(List<HandCardData> handData)
        {
            foreach (var cardData in handData)
            {
                var existingBehaviours = cardData.View.GetComponents<ITransformCastCardBehaviour>();
                foreach (var behaviour in existingBehaviours)
                {
                    if (behaviour is Component comp)
                    {
                        Object.Destroy(comp);
                    }
                }
                
                switch (cardData.Data.targetSpellType)
                {
                    case TargetSpellType.AnyTarget:
                        var selectObjectTarget =
                            _instantiator.InstantiateComponent<SelectTransformCastCardUseBehaviour>(
                                cardData.View.gameObject);

                        selectObjectTarget.Init(
                            cardData.View._cardContainer,
                            cardData.View._cursorArrowHead,
                            cardData.View._cursorArrowLine);

                        cardData.Behaviour = selectObjectTarget;
                        break;

                    default:
                        var nonTargetBehaviour =
                            _instantiator.InstantiateComponent<NonTransformCastCardUseBehaviour>(
                                cardData.View.gameObject);

                        cardData.Behaviour = nonTargetBehaviour;
                        break;
                }
            }
        }

        
        public void ChakraCheckCanCastCard(List<HandCardData> handData)
        {
            foreach (var cardData in handData)
            {
                cardData.Behaviour.CanCastCard(_gameSessionModel.PlayerHero.Chakra >= cardData.Data.Cost);
            }
        }
    }
}