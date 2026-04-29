using Feature.GameSessionData;

namespace Feature.ShopGamePlay.ItemShopSystem
{
    public interface IHealingModifier
    {
        int ModifyHealing(int baseHealing, CardAndHealthEntityOwnerData caster, CardAndHealthEntityOwnerData target);
    }
}