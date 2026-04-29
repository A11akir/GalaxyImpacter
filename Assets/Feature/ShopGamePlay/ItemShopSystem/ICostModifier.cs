using Feature.Card.Script;
using Feature.GameSessionData;

namespace Feature.ShopGamePlay.ItemShopSystem
{
    public interface ICostModifier
    {
        int ModifyCost(int baseCost, CardStatsData card, CardAndHealthEntityOwnerData owner);
    }
}