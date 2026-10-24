using System.Collections.Generic;
using Feature.GameSessionData;

namespace Feature.CardEffect.Script
{
    /// <summary>
    /// Модификатор целей эффекта. Вкладывается в эффект через SerializeReference
    /// (как IGeneratedCardModifier у генератора карт и ICritModifier у крита).
    /// Своё велью читает через context.Value().
    /// </summary>
    public interface ITargetModifier
    {
        List<CardAndHealthEntityOwnerData> ModifyTargets(
            List<CardAndHealthEntityOwnerData> targets, EffectContext context);
    }
}
