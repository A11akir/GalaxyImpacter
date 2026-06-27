// HandCardPresenter.cs — убираем оба метода ChakraCheckCanCast*, оставляем только то, что реально его касается
using R3;
using Feature.GameSessionData;

namespace Feature.Card.Script
{
    public class HandCardPresenter
    {
        public void RemoveCardFromHand(HandCardView view, HandCardViews handCardViews)
        {
            handCardViews.RemoveHandCardView(view);
        }

        public void ActivatePassiveEffects(
            HandCardView view,
            CardStatsData cardData,
            CardAndHealthEntityOwnerData owner,
            HandCardData handCardData, // ← теперь нужна только ОДНА карта, не вся рука (вернёмся к этому в проблеме №2)
            HandCardCastabilitySystem castabilitySystem)
        {
            var composite = new CompositeDisposable();

            foreach (var effect in cardData.PassiveCardEffects)
            {
                var sub = effect.Activate(owner, cardData, () =>
                {
                    view.SetCost(cardData.Cost);
                    castabilitySystem.Refresh(handCardData, owner.Chakra);
                });
                composite.Add(sub);
            }

            view.SetPassiveSubscriptions(composite);
        }
    }
}