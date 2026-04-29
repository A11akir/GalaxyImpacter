using System;
using System.Collections.Generic;
using Feature.Items.Scripts;
using Feature.ShopGamePlay.Script.ShopWindow;

public class ShopGameplayPresenter
{
    private ShopGameplayView _shopGameplayView;

    public event Action<ItemData, ItemShopView> OnItemClicked;
    
    public ShopGameplayPresenter(ShopGameplayView shopGameplayView)
    {
        _shopGameplayView = shopGameplayView;
        InitializeItemViews();
    }

    private void InitializeItemViews()
    {
        var itemViews = _shopGameplayView.GetItemViews();
        
        foreach (var itemView in itemViews)
        {
            itemView.OnItemClicked += (data) => OnItemClicked?.Invoke(data, itemView);
        }
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
        var itemViews = _shopGameplayView.GetItemViews();
        
        for (int i = 0; i < itemViews.Count && i < gameDataAllItems.Count; i++)
        {
            itemViews[i].SetView(gameDataAllItems[i]);
        }
    }
}