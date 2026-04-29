using Feature.GameSessionData;
using Feature.Items.Scripts;
using UnityEngine;

namespace Feature.ShopGamePlay.ItemShopSystem
{
    public class ItemShopSystem
    {
        private readonly GameSessionModel _gameSessionModel;
        
        public ItemShopSystem(GameSessionModel gameSessionModel)
        {
            _gameSessionModel = gameSessionModel;
        }
        
        public void TryPurchaseItem(ItemData item, ItemShopView itemView)
        {
            var playerData = _gameSessionModel.PlayerHero;
            
            if (!CanAfford(playerData, item))
            {
                Debug.LogWarning($"[ItemShop] Not enough gold! Need {item.GoldCost}, have {playerData.Currency}");
                itemView.PlayCannotAffordAnimation();
                return;
            }
            
            PurchaseItem(playerData, item);
            itemView.PlayPurchaseAnimation();
        }
        
        private bool CanAfford(GameSessionPlayerData playerData, ItemData item)
        {
            return playerData.Currency >= item.GoldCost;
        }
        
        private void PurchaseItem(GameSessionPlayerData playerData, ItemData item)
        {
            playerData.Currency -= item.GoldCost;
            
            // Добавляем предмет в инвентарь
            playerData.Inventory.AddItem(item);
            
            Debug.Log($"[ItemShop] Purchased {item.ItemName} for {item.GoldCost} gold. Inventory size: {playerData.Inventory.Items.CurrentValue.Count}");
        }
    }
    
    // Интерфейс для модификаторов урона

    // Интерфейс для модификаторов лечения

    // Интерфейс для триггерных эффектов

    // Интерфейс для модификаторов стоимости
}