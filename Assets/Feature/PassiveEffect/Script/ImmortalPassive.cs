using System;
using Feature.CombatSystem;
using Feature.GameSessionData;

namespace Feature.PassiveEffect.Script
{
    /// <summary>
    /// Бессмертие: летальный урон не наносится — цель выживает с текущим здоровьем.
    /// </summary>
    [Serializable]
    public class ImmortalPassive : PassiveEffectBase, IDamageReaction
    {
        public int Priority => 0;

        public bool ReactToDamage(
            CardAndHealthEntityOwnerData target, ref int finalDamage, CardAndHealthEntityOwnerData source) =>
            target.HealthValue - finalDamage <= 0;

        public override void Register(CardAndHealthEntityOwnerData owner) { }
        public override void Unregister() { }

        public override PassiveEffectBase Clone()
        {
            var clone = new ImmortalPassive();
            CopyBaseFields(clone);
            return clone;
        }
    }
}
