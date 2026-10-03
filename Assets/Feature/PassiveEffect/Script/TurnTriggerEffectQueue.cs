using System.Collections.Generic;
using Feature.CardEffect.Script;

namespace Feature.PassiveEffect.Script
{
    public class TurnTriggerEffectQueue
    {
        private readonly Queue<TurnTriggerEffectPassiveBase> _turnEndQueue = new();
        private readonly Queue<TurnTriggerEffectPassiveBase> _turnStartQueue = new();

        public void Enqueue(TurnTriggerEffectPassiveBase passive)
        {
            var queue = passive.Timing == TurnTriggerTiming.TurnEnd ? _turnEndQueue : _turnStartQueue;
            queue.Enqueue(passive);
        }

        public void TriggerAll(TurnTriggerTiming timing)
        {
            var queue = timing == TurnTriggerTiming.TurnEnd ? _turnEndQueue : _turnStartQueue;

            while (queue.Count > 0)
                queue.Dequeue().TriggerEffects();
        }

        public void Clear()
        {
            _turnEndQueue.Clear();
            _turnStartQueue.Clear();
        }
    }
}