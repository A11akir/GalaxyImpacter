using System;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class TurnEndEffectPassive : TurnTriggerEffectPassiveBase
    {
        public override TurnTriggerTiming Timing => TurnTriggerTiming.TurnEnd;

        public TurnEndEffectPassive() => Duration = PassiveEffect.Script.DurationType.UntilTurnEnd;

        public override PassiveEffect.Script.PassiveEffectBase Clone() =>
            new TurnEndEffectPassive { Effects = Effects, Config = Config, Duration = Duration, UseSourceCardAsDescription = UseSourceCardAsDescription };
    }
}