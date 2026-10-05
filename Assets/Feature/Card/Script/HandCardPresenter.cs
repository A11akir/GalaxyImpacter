using System;
using Feature.CardEffect.Script;
using R3;
using Feature.GameSessionData;

namespace Feature.Card.Script
{
    public class HandCardPresenter
    {
        public void RemoveCardFromHand(HandCardView view, HandCardViews handCardViews) => 
            handCardViews.RemoveHandCardView(view);

        public IDisposable ActivatePassiveEffects(CardStatsData cardData, CardAndHealthEntityOwnerData owner)
        {
            var composite = new CompositeDisposable();

            foreach (var effect in cardData.PassiveCardEffects)
                composite.Add(effect.Activate(owner, cardData));

            return composite;
        }
    }
}
