using Feature.GameSessionData;

namespace Feature.CombatSystem
{
    public interface IDamageReaction
    {
        bool ReactToDamage(CardAndHealthEntityOwnerData target, int finalDamage, CardAndHealthEntityOwnerData source);
    }
}