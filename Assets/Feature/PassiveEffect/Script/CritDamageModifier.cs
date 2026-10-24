using System;
using Feature.CardEffect.Script;

namespace Feature.PassiveEffect.Script
{
    /// <summary>
    /// Модификатор урона крита: своё велью прибавляется к проценту урона ауры
    /// (например +50 поверх 150 = 200%).
    /// </summary>
    [Serializable]
    public class CritDamageModifier : ICritModifier
    {
        public void Apply(CritState state, EffectContext context) =>
            state.DamagePercent += context.Value();
    }
}
