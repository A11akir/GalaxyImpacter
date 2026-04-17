using Feature.Battlefield.Script;
using Feature.GameSessionData;
using Feature.GoogleSheets;

namespace Feature.CardEffect.Script
{
    public class EffectContext
    {
        public CardAndHealthEntityOwnerData Caster;
        public CardAndHealthEntityOwnerData Target;
        public GameSessionModel GameSessionModel;
        public BattlefieldSystem BattlefieldSystem;
        public SpellCardData CardData;
        public int ValueIndex;
    }
}