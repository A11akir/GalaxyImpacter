namespace Feature.PassiveEffect.Script
{
    public enum DurationType
    {
        Permanent,      // живёт пока явно не снят
        UntilTurnEnd,   // снимается в конце текущего хода
        Turns 
    }
}