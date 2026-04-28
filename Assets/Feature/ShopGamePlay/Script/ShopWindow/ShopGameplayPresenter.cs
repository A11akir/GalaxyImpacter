using System.Collections.Generic;
using Feature.Items.Scripts;

namespace Feature.ShopGamePlay.Script.ShopWindow
{
    public class ShopGameplayPresenter
    {
        private ShopGameplayView _shopGameplayView;

        public ShopGameplayPresenter(ShopGameplayView shopGameplayView)
        {
            _shopGameplayView = shopGameplayView;
        }

        public void UnlockShop()
        {
            _shopGameplayView.UnlockShop();
        }

        public void LockShop()
        {
            _shopGameplayView.LockShop();
        }

        public void RefreshViewShop(List<ItemData> gameDataAllItems)
        {
            _shopGameplayView.RefreshViewShop(gameDataAllItems);
        }
    }
}