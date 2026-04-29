using Feature.Data;
using Feature.Items.Scripts;

namespace Feature.ShopGamePlay.Script.ShopWindow
{
    public class ShopGameplaySystem
    {
        private ShopGameplayPresenter _shopGameplayPresenter;
        private ItemShopSystem.ItemShopSystem _itemShopSystem;
        private GameData _gameData;
        
        public ShopGameplaySystem(
            ShopGameplayPresenter shopGameplayPresenter, 
            GameData gameData,
            ItemShopSystem.ItemShopSystem itemShopSystem)
        {
            _shopGameplayPresenter = shopGameplayPresenter;
            _gameData = gameData;
            _itemShopSystem = itemShopSystem;
            
            _shopGameplayPresenter.OnItemClicked += HandleItemClicked;
        }

        public void UnlockShop()
        {
            _shopGameplayPresenter.UnlockShop();
        }

        public void LockShop()
        {
            _shopGameplayPresenter.LockShop();
        }

        public void RefreshShop()
        {
            _shopGameplayPresenter.RefreshViewShop(_gameData.allItems);
        }
        
        private void HandleItemClicked(ItemData item, ItemShopView itemView)
        {
            _itemShopSystem.TryPurchaseItem(item, itemView);
        }
    }
}