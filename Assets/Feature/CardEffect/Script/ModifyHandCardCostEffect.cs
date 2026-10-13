using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Feature.CardEffect.Script
{
    public enum CostChangeDirection
    {
        Reduce,   // 0 — значение, записанное в ассетах
        Increase
    }

    [Serializable]
    public class ModifyHandCardCostEffect : CardEffect
    {
        [FormerlySerializedAs("_query")] [SerializeField] private CardPickFilter filter = new();
        [SerializeField] private CostChangeDirection _direction;

        public override void Execute(EffectContext context)
        {
            var values = context.CardData.Values;
            if (values == null || context.ValueIndex >= values.Count) return;

            int amount = values[context.ValueIndex];
            if (amount == 0) return;

            // Режим модификатора сгенерированной карты: вложен в GenerateTemporaryHandCardEffect.
            if (context.TargetCard != null)
            {
                ModifyCost(context.TargetCard, amount);
                return;
            }

            foreach (var handOwner in ResolveTargets(context))
            {
                if (handOwner == null) continue;

                var card = context.CardPoolPickSystem.PickFromHand(handOwner, filter, context);
                if (card == null) continue;

                ModifyCost(card, amount);
            }
        }

        private void ModifyCost(CardStatsData card, int amount)
        {
            card.Cost = _direction == CostChangeDirection.Reduce
                ? Mathf.Max(0, card.Cost - amount)
                : card.Cost + amount;
        }
    }
}
