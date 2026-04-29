using Feature.GameSessionData;

namespace Feature.ShopGamePlay.ItemShopSystem
{
    public interface IDamageModifier
    {
        int ModifyDamage(int baseDamage, CardAndHealthEntityOwnerData caster, CardAndHealthEntityOwnerData target);
    }
}