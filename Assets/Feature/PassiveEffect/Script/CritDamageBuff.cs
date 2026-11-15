using System;

namespace Feature.PassiveEffect.Script
{
    /// <summary>
    /// Крит-бафф урона: +n% к проценту урона крита.
    /// Конфиг: «Критический урон увеличен на n% (минимальный шанс 10%)».
    /// </summary>
    [Serializable]
    public class CritDamageBuff : CritBuff
    {
        public override int DamageBonus => Value.CurrentValue;

        public override PassiveEffectBase Clone()
        {
            var clone = new CritDamageBuff();
            CopyFields(clone);
            return clone;
        }
    }
}
