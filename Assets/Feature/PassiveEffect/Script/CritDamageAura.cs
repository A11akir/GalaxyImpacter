using System;

namespace Feature.PassiveEffect.Script
{
    /// <summary>
    /// Крит: умножает урон на процент из Value. Value=150 → 150% (урон 10 → 15).
    /// Плоский бонус не даёт, только множитель.
    /// </summary>
    [Serializable]
    public class CritDamageAura : DamageBonusAura
    {
        public override int GetDamageBonus(CardStatsData sourceCard) => 0;

        public override float GetDamageMultiplier(CardStatsData sourceCard) =>
            Value.CurrentValue / 100f;

        public override PassiveEffectBase Clone() =>
            new CritDamageAura { Config = Config, Duration = Duration, TargetSelection = TargetSelection };
    }
}
