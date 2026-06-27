
using System.Collections.Generic;
using Feature.GameSessionData;
using R3;
using UnityEngine;


namespace Feature.Card.Script
{
    public class HandCardPresenter
    {
        
        public void ChakraCheckCanCastHand(List<HandCardData> handCardData, int chakra)
        {
            foreach (var cardData in handCardData)
                ChakraCheckCanCastCard(cardData, chakra);
        }

        public void ChakraCheckCanCastCard(HandCardData cardData, int chakra)
        {
            cardData.View.SetCanCastView(chakra >= cardData.Data.Cost);
        }

        public void RemoveCardFromHand(HandCardView view, HandCardViews handCardViews)
        {
            handCardViews.RemoveHandCardView(view);
        }
        
        public void ActivatePassiveEffects(
            HandCardView view,
            CardStatsData cardData,
            CardAndHealthEntityOwnerData owner,
            List<HandCardData> handData,
            FactoryHandBehaviourTransformCastSystem factory)
        {
            var composite = new CompositeDisposable();

            foreach (var effect in cardData.PassiveCardEffects)
            {
                var sub = effect.Activate(owner, cardData, () =>
                {
                    view.SetCost(cardData.Cost);
                    factory.ChakraCheckCanCastCard(handData, owner.Chakra); // ← логика (можно драгать)
                    ChakraCheckCanCastHand(handData, owner.Chakra);          // ← визуал (подсветка)
                });
                composite.Add(sub);
            }

            view.SetPassiveSubscriptions(composite);
        }
    }
}