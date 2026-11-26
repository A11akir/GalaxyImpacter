using Feature.Hero;

namespace Feature.PassiveEffect.Script
{
    public static class CardClassCheck
    {
        public static bool Has(CardStatsData card, AllHeroClass heroClass) =>
            card != null && card.Specialization != null && card.Specialization.Contains(heroClass);
    }
}
