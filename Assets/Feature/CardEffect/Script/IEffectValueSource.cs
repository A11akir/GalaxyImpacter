namespace Feature.CardEffect.Script
{
    /// <summary>
    /// Источник значения для эффекта: вместо велью карты отдаёт число из игры
    /// (прочность щита, здоровье, количество существ и т.д.).
    /// </summary>
    public interface IEffectValueSource
    {
        int GetValue(EffectContext context);
    }
}
