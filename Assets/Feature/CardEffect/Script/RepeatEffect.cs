using System;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class RepeatEffect : CardEffect
    {
        public override void Execute(EffectContext context)
        {
            int repeatCount = context.CardData.Values[context.ValueIndex]-1;
            int previousIndex = context.ValueIndex - 1;

            var previousEffect = context.CardData.Effects[previousIndex];

            int savedIndex = context.ValueIndex;
            context.ValueIndex = previousIndex; // ← подменяем индекс на момент повторного вызова

            for (int i = 0; i < repeatCount; i++)
                previousEffect.Execute(context);

            context.ValueIndex = savedIndex; // ← восстанавливаем (на случай если после RepeatEffect есть ещё эффекты)
        }
    }
}