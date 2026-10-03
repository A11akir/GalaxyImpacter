using System;
using Feature.PassiveEffect.Script;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class TurnStartEffectPassive : TurnTriggerEffectPassiveBase
    {
        public override TurnTriggerTiming Timing => TurnTriggerTiming.TurnStart;

        public TurnStartEffectPassive() => Duration = PassiveEffect.Script.DurationType.Permanent;

        public override PassiveEffectBase Clone() =>
            new TurnStartEffectPassive { Effects = Effects, Config = Config, Duration = Duration, UseSourceCardAsDescription = UseSourceCardAsDescription };
    }
}