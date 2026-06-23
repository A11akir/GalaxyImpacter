
using System.Collections.Generic;
using Feature.CardEffect.Script;
using UnityEngine;

namespace Feature.PassiveEffect.Script
{
    public class TurnEndEffectQueue
    {
        private readonly Queue<TurnEndEffectPassive> _queue = new();

// TurnEndEffectQueue.cs
        public void Enqueue(TurnEndEffectPassive passive)
        {
            _queue.Enqueue(passive);
            Debug.Log($"[TurnEndEffectQueue] Enqueue called, queue size now={_queue.Count}");
        }

        public void TriggerAll()
        {
            Debug.Log($"[TurnEndEffectQueue] TriggerAll called, queue size={_queue.Count}");
            while (_queue.Count > 0)
            {
                var passive = _queue.Dequeue();
                Debug.Log("[TurnEndEffectQueue] Dequeue and TriggerEffects");
                passive.TriggerEffects();
            }
        }

        public void Clear() => _queue.Clear();
    }
}