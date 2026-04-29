using Feature.GameSessionData;

namespace Feature.ShopGamePlay.ItemShopSystem
{
    public interface ITriggerEffect
    {
        ItemTriggerType TriggerType { get; }
        void Execute(CardAndHealthEntityOwnerData owner);
    }
}