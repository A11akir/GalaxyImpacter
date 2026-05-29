using System;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class DrawCardsEffect : CardEffect
    {
        public override void Execute(EffectContext context)
        {
            for (int i = 0; i < context.CardData.Values[context.ValueIndex]; i++)
            {
                context.Caster.DrawCardFromDeck();
            }
                
        }
    }
}