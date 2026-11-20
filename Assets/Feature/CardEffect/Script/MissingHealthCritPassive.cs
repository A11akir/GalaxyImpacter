using System;
using Feature.GameSessionData;
using Feature.PassiveEffect.Script;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class MissingHealthCritPassive : CritBuff
    {
        public override int DamageBonus => Value.CurrentValue;
        public override void Register(CardAndHealthEntityOwnerData owner) { }
        public override void Unregister() { }
        public override PassiveEffectBase Clone()
        {
            var clone = new MissingHealthCritPassive();
            CopyBaseFields(clone);
            return clone;
        }
    }
}