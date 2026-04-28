using Feature.Data;

namespace Feature.ShopGamePlay.Script.ShopWindow
{
    public class ShopGameplaySystem
    {
        private ShopGameplayPresenter _shopGameplayPresenter;
        
        private GameData _gameData;
        public ShopGameplaySystem(ShopGameplayPresenter shopGameplayPresenter, GameData gameData)
        {
            _shopGameplayPresenter = shopGameplayPresenter;
            _gameData = gameData;
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
    }
}