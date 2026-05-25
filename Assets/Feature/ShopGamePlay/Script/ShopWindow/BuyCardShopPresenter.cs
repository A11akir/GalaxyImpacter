using System.Collections.Generic;
using Feature.Card.Script;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using UnityEngine;

namespace Feature.ShopGamePlay.Script.ShopWindow
{
    public class BuyCardShopPresenter
    {
        private readonly CardsShopContainerView _containerView;
        private readonly ShopCardOfferSystem _cardOfferSystem;
        private readonly BuyCardShopSystem _buyCardSystem;
        private readonly GameSessionModel _gameSessionModel;

        public BuyCardShopPresenter(
            CardsShopContainerView containerView,
            ShopCardOfferSystem cardOfferSystem,
            BuyCardShopSystem buyCardSystem,
            GameSessionModel gameSessionModel)
        {
            _containerView = containerView;
            _cardOfferSystem = cardOfferSystem;
            _buyCardSystem = buyCardSystem;
            _gameSessionModel = gameSessionModel;

            foreach (var buyView in _containerView.GetBuyViews())
                buyView.OnCardClicked += HandleCardClicked;
        }

        public void RefreshCardOffers()
        {
            var offers = _cardOfferSystem.GenerateCardOffers();
            _containerView.SetCards(offers);
        }

        private void HandleCardClicked(CardStatsData card, CardBuyShopView clickedView)
        {
            var playerData = _gameSessionModel.PlayerHero;

            if (playerData.Currency < card.Cost)
            {
                clickedView.PlayCannotAffordAnimation();
                return;
            }

            playerData.Currency -= card.Cost;
            _buyCardSystem.BuyCard(card);
            clickedView.PlayPurchaseAnimation();
        }
    }
}