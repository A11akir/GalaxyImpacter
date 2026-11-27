using Feature.GameSessionData;

namespace Feature.PassiveEffect.Script
{
    public interface IHealReaction
    {
        int Priority { get; }

        /// <summary>Можно ли восстановить HP цели. false — лечение не применяется.</summary>
        bool CanHeal(CardAndHealthEntityOwnerData target, int heal);
    }
}
