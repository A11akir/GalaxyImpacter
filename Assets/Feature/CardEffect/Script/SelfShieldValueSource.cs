using System;

namespace Feature.CardEffect.Script
{
    /// <summary>Значение = прочность щита кастера.</summary>
    [Serializable]
    public class SelfShieldValueSource : IEffectValueSource
    {
        public int GetValue(EffectContext context) => context.Caster.ShieldValue;
    }
}
