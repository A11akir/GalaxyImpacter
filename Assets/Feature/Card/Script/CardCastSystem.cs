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
        
        public void ChakraCheckCanCastCard(List<HandCardData> handData, int chakra)
        {
            foreach (var cardData in handData)
                cardData.Behaviour.CanCastCard(chakra >= cardData.Data.Cost);
        }
        
        public void AddBehavioursToCard(HandCardData cardData)
        {
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

        public void RemoveBehaviourFromCard(HandCardData cardData)
        {
            if (cardData?.Behaviour != null)
            {
                Object.Destroy(cardData.Behaviour as MonoBehaviour);
                cardData.Behaviour = null;
            }
        }
    }
}