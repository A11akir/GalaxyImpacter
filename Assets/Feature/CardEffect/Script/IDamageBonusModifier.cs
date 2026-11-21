namespace Feature.CardEffect.Script
{
    /// <summary>
    /// Модификатор, который добавляет плоский урон атаке (поверх велью эффекта).
    /// </summary>
    public interface IDamageBonusModifier
    {
        int DamageBonus { get; }
    }
}
