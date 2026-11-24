using System;
using Feature.PassiveEffect.Script;

namespace Feature.CardEffect.Script
{
    /// <summary>Значение = текущий бонус FireDamageBonus кастера.</summary>
    [Serializable]
    public class FireBonusValueSource : IEffectValueSource
    {
        public int GetValue(EffectContext context)
        {
            var bonus = context.Caster?.PassiveEffects.Find<FireDamageBonus>();

            return bonus != null ? bonus.Value.CurrentValue : 0;
        }
    }
}
