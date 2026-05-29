using System;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public abstract class CardEffect
    {
        public abstract void Execute(EffectContext context);
    }
}
