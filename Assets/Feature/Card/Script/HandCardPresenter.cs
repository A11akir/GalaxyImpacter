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

        public void ActivatePassiveEffects(
            HandCardView view,
            CardStatsData cardData,
            CardAndHealthEntityOwnerData owner,
            Action<PassiveCardEffect> onEffectChanged)
        {
            var composite = new CompositeDisposable();

            foreach (var effect in cardData.PassiveCardEffects)
            {
                var sub = effect.Activate(owner, cardData, () =>
                {
                    view.SetDataView(cardData);
                    onEffectChanged?.Invoke(effect);
                });
                composite.Add(sub);
            }

            view.SetPassiveSubscriptions(composite);
        }
    }
}