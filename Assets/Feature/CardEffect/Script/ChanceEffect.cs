using System;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class ChanceEffect : CardEffect
    {
        // Эффект, который выполняется при успешном броске. Вложен, как в ZoneEffect:
        // контейнер читает Values[ValueIndex], вложенный эффект — Values[ValueIndex + 1].
        [SerializeReference] private CardEffect _effect;

        public override void Execute(EffectContext context)
        {
            int chancePercent = context.Value();

            if (UnityEngine.Random.Range(0, 100) >= chancePercent) return;

            ExecuteInSlot(context, 1, _effect.Execute);
        }
    }
}
