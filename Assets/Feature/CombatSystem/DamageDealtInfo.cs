using Feature.GameSessionData;

namespace Feature.CombatSystem
{
    public struct DamageDealtInfo
    {
        public CardAndHealthEntityOwnerData Source;
        public CardAndHealthEntityOwnerData Target;
        public CardStatsData SourceCard;
        public int Amount;
    }
}
