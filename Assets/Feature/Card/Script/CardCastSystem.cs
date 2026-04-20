using Feature.GameSessionData;
using System.Collections.Generic;
using Feature.HandLogic;
using Feature.Hero;
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
        
        public void AddBehavioursToCard(HandCardData cardData, bool isHeroPower = false)
        {
            switch (cardData.Data.TargetType)
            {
                case TargetType.AnyTarget:
                    var selectObjectTarget =
                        _instantiator.InstantiateComponent<SelectTransformCastCardUseBehaviour>(
                            cardData.View.gameObject);

                    if (isHeroPower && cardData.View is HeroPowerView heroPowerView)
                        selectObjectTarget.Init(
                            heroPowerView._heroPowerContainer,
                            heroPowerView._heroPowerArrowHead,
                            heroPowerView._heroPowerArrowLine);
                    else
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
                            
            if (cardData.Behaviour is NonTransformCastCardUseBehaviour nonTarget)
                nonTarget.IsHeroPower = isHeroPower;
            else if (cardData.Behaviour is SelectTransformCastCardUseBehaviour select)
                select.IsHeroPower = isHeroPower;
        }

        public void RemoveBehaviourFromHandCard(HandCardData cardData)
        {
            if (cardData?.Behaviour != null)
            {
                Object.Destroy(cardData.Behaviour as MonoBehaviour);
                cardData.Behaviour = null;
            }
        }
    }
}