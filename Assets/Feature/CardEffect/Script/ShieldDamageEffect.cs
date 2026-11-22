using System;

namespace Feature.CardEffect.Script
{
    /// <summary>
    /// Урон = велью карты + значение источника (например прочность своего щита).
    /// Карта наносит урон и без щита — за счёт велью.
    /// </summary>
    [Serializable]
    public class ShieldDamageEffect : DamageEffect
    {
        protected override int GetBaseValue(EffectContext context) =>
            context.Value() + (_valueSource != null ? _valueSource.GetValue(context) : 0);
    }
}
