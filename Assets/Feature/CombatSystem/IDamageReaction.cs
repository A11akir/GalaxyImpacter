using Feature.GameSessionData;

namespace Feature.CombatSystem
{
    public interface IDamageReaction
    {
        int Priority { get; }
        bool ReactToDamage(CardAndHealthEntityOwnerData target, ref int finalDamage, CardAndHealthEntityOwnerData source);
    }
}
