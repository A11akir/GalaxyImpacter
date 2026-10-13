using System;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class ChanceEffect : CardEffect
    {
        // Сколько следующих эффектов карты выполняется только при успешном броске.
        // Число каждого эффекта берётся из Values по его собственной позиции.
        [SerializeField] private int _affectedCount = 1;

        public override int ControlledFollowingCount => Mathf.Max(0, _affectedCount);

        public override void Execute(EffectContext context)
        {
            int chancePercent = context.CardData.Values[context.ValueIndex];

            if (UnityEngine.Random.Range(0, 100) >= chancePercent) return;

            ExecuteFollowingEffects(context, _affectedCount);
        }
    }
}
