using Feature.GameSessionData;

namespace Feature.PassiveEffect.Script
{
    public interface IDamageReaction
    {
        int Priority { get; }
        bool ReactToDamage(CardAndHealthEntityOwnerData target, int finalDamage, CardAndHealthEntityOwnerData source);
    }
}