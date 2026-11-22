using System;
using Feature.GameSessionData;

namespace Feature.Card.Script
{
    /// <summary>Цель с неполным здоровьем (текущее меньше максимума).</summary>
    [Serializable]
    public class IncompleteHealthTargetCheck : TargetCheck
    {
        public override bool CanPick(
            CardAndHealthEntityOwnerData target,
            CardAndHealthEntityOwnerData caster,
            GameSessionModel model) =>
            target.HealthValue < target.MaxHealth;
    }
}
