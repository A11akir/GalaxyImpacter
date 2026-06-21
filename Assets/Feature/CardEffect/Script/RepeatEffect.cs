using System;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class RepeatEffect : CardEffect
    {
        public override void Execute(EffectContext context)
        {

            int repeatCount = context.CardData.Values[context.ValueIndex] - 1;
            int previousIndex = context.ValueIndex - 1;
            
            var previousEffect = context.CurrentEffectsList[previousIndex];

            int savedIndex = context.ValueIndex;
            context.ValueIndex = previousIndex;

            for (int i = 0; i < repeatCount; i++)
                previousEffect.Execute(context);

            context.ValueIndex = savedIndex;
        }
    }
}