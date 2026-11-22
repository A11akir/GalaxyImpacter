using System;

namespace Feature.CardEffect.Script
{
    /// <summary>
    /// Убирает из коллекции кастера карту, которая вызвала этот эффект.
    /// Если карты нет в коллекции (сгенерированная) — ничего не происходит.
    /// </summary>
    [Serializable]
    public class RemoveCastCardEffect : CardEffect
    {
        public override void Execute(EffectContext context)
        {
            context.Caster.RemoveFromCollection(context.CardData.SourceId);
        }
    }
}
