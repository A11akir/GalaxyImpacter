using System;
using Feature.CardEffect.Script;

namespace Feature.PassiveEffect.Script
{
    /// <summary>
    /// Модификатор крита, вкладывается в CritDamageAura (как IGeneratedCardModifier
    /// в генератор карт). Читает своё велью через context.Value() и пишет в CritState.
    /// </summary>
    public interface ICritModifier
    {
        void Apply(CritState state, EffectContext context);
    }

    /// <summary>Состояние крита, которое модификаторы наполняют.</summary>
    [Serializable]
    public class CritState
    {
        /// <summary>Шанс крита в процентах. 100 = всегда крит (поведение без модификаторов).</summary>
        public int ChancePercent = 100;

        /// <summary>Дополнительный процент урона поверх велью самой ауры.</summary>
        public int DamagePercent;
    }
}
