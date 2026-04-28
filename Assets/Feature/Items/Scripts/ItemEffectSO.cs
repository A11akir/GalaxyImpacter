using Feature.CardEffect.Script;
using UnityEngine;

namespace Feature.Items.Scripts
{
    public abstract class ItemEffectSO : ScriptableObject
    {
        public abstract void Execute(EffectContext context);
    }
}