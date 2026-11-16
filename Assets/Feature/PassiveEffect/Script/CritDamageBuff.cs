using System;

namespace Feature.PassiveEffect.Script
{
    [Serializable]
    public class CritDamageBuff : CritBuff
    {
        public override int DamageBonus => Value.CurrentValue;

        public override PassiveEffectBase Clone()
        {
            var clone = new CritDamageBuff();
            CopyFields(clone);
            return clone;
        }
    }
}
