using System.Collections.Generic;
using Feature.Card.Script;
using Feature.GoogleSheets;
using UnityEngine;

namespace Feature.ShopGamePlay.Script.ShopWindow
{
    public class BuyCardShopPresenter
    {
        private readonly CardsShopContainerView _containerView;
        private readonly ShopCardOfferSystem _cardOfferSystem;
        private readonly BuyCardShopSystem _buyCardSystem;

        public BuyCardShopPresenter(
            CardsShopContainerView containerView,
            ShopCardOfferSystem cardOfferSystem,
            BuyCardShopSystem buyCardSystem)
        {
            _containerView = containerView;
            _cardOfferSystem = cardOfferSystem;
            _buyCardSystem = buyCardSystem;

            foreach (var buyView in _containerView.GetBuyViews())
                buyView.OnCardClicked += HandleCardClicked;
        }

        public void RefreshCardOffers()
        {
            var offers = _cardOfferSystem.GenerateCardOffers();
            
            _containerView.SetCards(offers);
        }

        private void HandleCardClicked(CardStatsData card)
        {
            _buyCardSystem.BuyCard(card);

            var buyViews = _containerView.GetBuyViews();
            foreach (var view in buyViews)
            {
                // TODO: передать view в BuyCardShopSystem для анимации
                view.PlayPurchaseAnimation();
                break;
            }
        }
    }
}