using System;

namespace Feature.PassiveEffect.Script
{
    /// <summary>
    /// Крит-бафф шанса: +n% к шансу крита.
    /// Конфиг: «Шанс нанесения критического урона увеличен на n%».
    /// </summary>
    [Serializable]
    public class CritChanceBuff : CritBuff
    {
        public override int ChanceBonus => Value.CurrentValue;

        public override PassiveEffectBase Clone()
        {
            var clone = new CritChanceBuff();
            CopyFields(clone);
            return clone;
        }
    }
}
