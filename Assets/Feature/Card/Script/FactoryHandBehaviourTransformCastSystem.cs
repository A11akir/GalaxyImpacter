using Feature.GameSessionData;
using System.Collections.Generic;
using Feature.Common;
using Feature.HandLogic;
using Feature.Hero;
using UnityEngine;
using Zenject;

namespace Feature.Card.Script
{
    public class FactoryHandBehaviourTransformCastSystem
    {
        private IInstantiator _instantiator;
        
        private readonly CursorArrowData _cursorArrowData;

        public FactoryHandBehaviourTransformCastSystem(IInstantiator instantiator, CursorArrowData cursorArrowData)
        {
            _instantiator = instantiator;
            _cursorArrowData = cursorArrowData;
        }
        

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
                        _cursorArrowData.ArrowHead,
                        _cursorArrowData.ArrowLine);

                    cardData.Behaviour = selectObjectTarget;
                    break;

                case TargetType.All:
                    cardData.Behaviour =
                        _instantiator.InstantiateComponent<NonTransformCastCardUseBehaviour>(
                            cardData.View.gameObject);
                    break;

                case TargetType.Hero:
                    cardData.Behaviour =
                        _instantiator.InstantiateComponent<HeroTransformCastCardUseBehaviour>(
                            cardData.View.gameObject);
                    break;
            }
        }

        public void AddBehavioursToHeroPower(HandCardData cardData, GameObject heroPowerObject)
        {
            switch (cardData.Data.TargetType)
            {
                case TargetType.AnyTarget:
                    var behaviour = _instantiator.InstantiateComponent<SelectTargetHeroPowerCastBehaviour>(heroPowerObject);
                    behaviour.Init(_cursorArrowData.ArrowHead, _cursorArrowData.ArrowLine);
                    cardData.Behaviour = behaviour;
                    break;
                    break;
                case TargetType.All:
                case TargetType.Hero:
                    cardData.Behaviour = _instantiator.InstantiateComponent<NonTargetHeroPowerCastBehaviour>(heroPowerObject);
                    break;
            }
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