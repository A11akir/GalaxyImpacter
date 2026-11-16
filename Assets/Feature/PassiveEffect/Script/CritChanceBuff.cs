using System;

namespace Feature.PassiveEffect.Script
{

    [Serializable]
    public class CritChanceBuff : CritBuff
    {
        public override int ChanceBonus => Value.CurrentValue;

        public override PassiveEffectBase Clone()
        {
            var clone = new CritChanceBuff();
            CopyFields(clone);
            return clone;
        }
    }
}
