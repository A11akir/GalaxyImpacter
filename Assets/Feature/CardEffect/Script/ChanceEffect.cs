using System;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class ChanceEffect : CardEffect
    {
        // Эффект, который выполняется при успешном броске. Вложен через SerializeReference:
        // контейнер читает Values[ValueIndex], вложенный эффект — Values[ValueIndex + 1].
        [SerializeReference] private CardEffect _effect;

        public override void Execute(EffectContext context)
        {
            if (_effect == null) return;

            // нет велью под шанс — эффект просто не срабатывает
            var values = context.CardData?.Values;
            if (values == null || context.ValueIndex < 0 || context.ValueIndex >= values.Count) return;

            int chancePercent = values[context.ValueIndex];

            if (UnityEngine.Random.Range(0, 100) >= chancePercent) return;

            ExecuteInSlot(context, 1, _effect.Execute);
        }
    }
}
