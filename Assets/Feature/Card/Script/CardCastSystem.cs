using Feature.GameSessionData;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Feature.Card.Script
{
    public class CardCastSystem
    {
        private IInstantiator _instantiator;
        
        public CardCastSystem(IInstantiator instantiator) => _instantiator = instantiator;

        public void ChakraCheckCanCastCard(List<HandCardData> handData, int chakra)
        {
            foreach (var cardData in handData)
                cardData.Behaviour.CanCastCard(chakra >= cardData.Data.Cost);
        }
        
        public void AddBehavioursToCard(HandCardData cardData)
        {
            switch (cardData.Data.TargetType)
            {
                case TargetType.AnyTarget:
                    var selectObjectTarget =
                        _instantiator.InstantiateComponent<SelectTransformCastCardUseBehaviour>(
                            cardData.View.gameObject);

                    selectObjectTarget.Init(
                        cardData.View._cardContainer,
                        cardData.View._cursorArrowHead,
                        cardData.View._cursorArrowLine);

                    cardData.Behaviour = selectObjectTarget;
                    break;

                case TargetType.All:
                    var nonTargetBehaviour =
                        _instantiator.InstantiateComponent<NonTransformCastCardUseBehaviour>(
                            cardData.View.gameObject);

                    cardData.Behaviour = nonTargetBehaviour;
                    break; 
                case TargetType.Hero:
                    var heroBehaviour =
                        _instantiator.InstantiateComponent<HeroTransformCastCardUseBehaviour>(
                            cardData.View.gameObject);

                    cardData.Behaviour = heroBehaviour;
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