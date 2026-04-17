using UnityEngine;

namespace Feature.CardEffect.Script
{
    public abstract class CardEffectSO : ScriptableObject
    {
        public abstract void Execute(EffectContext context);
    }
}
