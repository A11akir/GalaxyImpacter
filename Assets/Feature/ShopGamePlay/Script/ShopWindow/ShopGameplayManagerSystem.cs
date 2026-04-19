namespace Feature.ShopGamePlay.Script.ShopWindow
{
    public class ShopGameplayManagerSystem
    {
        private ShopGameplayPresenter _shopGameplayPresenter;

        public ShopGameplayManagerSystem(ShopGameplayPresenter shopGameplayPresenter)
        {
            _shopGameplayPresenter = shopGameplayPresenter;
        }

        public void UnlockShop()
        {
            _shopGameplayPresenter.UnlockShop();
        }

        public void LockShop()
        {
            _shopGameplayPresenter.LockShop();
        }
    }
}