namespace Feature.PassiveEffect.Script
{
    public interface ITeamDamageModifier
    {
        int GetDamageBonus(CardStatsData sourceCard);
    }
}