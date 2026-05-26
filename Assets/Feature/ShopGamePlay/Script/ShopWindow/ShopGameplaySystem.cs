using Feature.Data;
using Feature.Items.Scripts;

namespace Feature.ShopGamePlay.Script.ShopWindow
{
    public class ShopGameplaySystem
    {
        private ShopGameplayPresenter _shopGameplayPresenter;
        private ItemShopSystem.ItemShopSystem _itemShopSystem;
        private readonly BuyCardShopPresenter _cardShopPresenter;
        private GameData _gameData;
        
        public ShopGameplaySystem(
            ShopGameplayPresenter shopGameplayPresenter, 
            GameData gameData,
            ItemShopSystem.ItemShopSystem itemShopSystem, BuyCardShopPresenter cardShopPresenter)
        {
            _shopGameplayPresenter = shopGameplayPresenter;
            _gameData = gameData;
            _itemShopSystem = itemShopSystem;
            _cardShopPresenter = cardShopPresenter;

            _shopGameplayPresenter.OnItemClicked += HandleItemClicked;
            _shopGameplayPresenter.OnRefreshRequested += RefreshShop;
        }

        public void UnlockShop()
        {
            _shopGameplayPresenter.UnlockShop();
            _cardShopPresenter.RefreshCardOffers();
        }

        public void LockShop()
        {
            _shopGameplayPresenter.LockShop();
        }

        public void RefreshShop()
        {
            _shopGameplayPresenter.RefreshViewShop(_gameData.allItems);
            _cardShopPresenter.RefreshCardOffers();
        }
        
        private void HandleItemClicked(ItemData item, ItemShopView itemView)
        {
            _itemShopSystem.TryPurchaseItem(item, itemView);
        }
    }
}