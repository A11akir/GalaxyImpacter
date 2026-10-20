using System;
using Feature.CardEffect.Script;
using Feature.Hero;

namespace Feature.PassiveEffect.Script
{
    /// <summary>
    /// Бонус урона только для себя, и только для огненных заклинаний.
    /// </summary>
    [Serializable]
    public class FireDamageBonus : DamageBonusAura
    {
        public FireDamageBonus()
        {
            TargetSelection = TargetSelectionType.Self;
            Duration = DurationType.UntilTurnEnd;
        }

        public override int GetDamageBonus(CardStatsData sourceCard)
        {
            if (!sourceCard || !sourceCard.Specialization.Contains(AllHeroClass.FireMage)) return 0;
            return base.GetDamageBonus(sourceCard);
        }

        public override PassiveEffectBase Clone() =>
            new FireDamageBonus { Config = Config };
    }
}
