namespace Feature.CardEffect.Script
{
    /// <summary>
    /// Эффект, который можно вложить в генератор карт: получает сгенерированную карту
    /// через EffectContext.TargetCard и модифицирует её.
    /// </summary>
    public interface IGeneratedCardModifier
    {
        void Execute(EffectContext context);
    }
}
