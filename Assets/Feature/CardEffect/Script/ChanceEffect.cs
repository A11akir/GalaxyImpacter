using System;
using System.Collections.Generic;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class ChanceEffect : CardEffect
    {
        [SerializeReference] private List<CardEffect> _effects = new();

        public override void Execute(EffectContext context)
        {
            int chancePercent = context.CardData.Values[context.ValueIndex];

            int roll = UnityEngine.Random.Range(0, 100);
            if (roll >= chancePercent) return;

            for (int i = 0; i < _effects.Count; i++)
            {
                context.ValueIndex = i;
                _effects[i].Execute(context);
            }
        }
    }
}