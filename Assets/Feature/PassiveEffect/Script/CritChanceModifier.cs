using System;
using Feature.CardEffect.Script;

namespace Feature.PassiveEffect.Script
{
    /// <summary>
    /// Модификатор шанса крита: своё велью = шанс в процентах (25 = 25%).
    /// </summary>
    [Serializable]
    public class CritChanceModifier : ICritModifier
    {
        public void Apply(CritState state, EffectContext context) =>
            state.ChancePercent = context.Value();
    }
}
