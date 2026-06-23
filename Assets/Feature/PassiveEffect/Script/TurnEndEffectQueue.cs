
using System.Collections.Generic;
using Feature.CardEffect.Script;

namespace Feature.PassiveEffect.Script
{
    public class TurnEndEffectQueue
    {
        private readonly Queue<TurnEndEffectPassive> _queue = new();
        
        public void Enqueue(TurnEndEffectPassive passive)
        {
            _queue.Enqueue(passive);
        }

        public void TriggerAll()
        {
            while (_queue.Count > 0)
            {
                var passive = _queue.Dequeue();
                passive.TriggerEffects();
            }
        }

        public void Clear() => _queue.Clear();
    }
}