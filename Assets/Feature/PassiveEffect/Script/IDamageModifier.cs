namespace Feature.PassiveEffect.Script
{
    public interface IDamageModifier
    {
        int GetDamageBonus(CardStatsData sourceCard);
    }
}
