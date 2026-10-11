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
                composite.Add(effect.BindPassiveEffect(owner, cardData));

            return composite;
        }

        // View — только отображение, подписка на цену — здесь.
        // onCostChanged нужен, чтобы вызывающий пересчитал то, что зависит от цены (кастуемость).
        public IDisposable BindCost(CardStatsData cardData, HandCardView view, Action onCostChanged)
        {
            var composite = new CompositeDisposable();

            cardData.CostReactive
                .Subscribe(cost =>
                {
                    view.SetCost(cost);
                    onCostChanged?.Invoke();
                })
                .AddTo(composite);

            return composite;
        }
    }
}
