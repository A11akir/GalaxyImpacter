using System;
using Feature.PassiveEffect.Script;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class TurnStartEffectPassive : TurnTriggerEffectPassiveBase
    {
        public override TurnTriggerTiming Timing => TurnTriggerTiming.TurnStart;

        public TurnStartEffectPassive() => Duration = PassiveEffect.Script.DurationType.Permanent;

        public override PassiveEffectBase Clone()
        {
            var clone = new TurnStartEffectPassive { Effects = Effects };
            CopyBaseFields(clone);
            return clone;
        }
    }
}