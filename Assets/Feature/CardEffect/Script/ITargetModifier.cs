using System.Collections.Generic;
using Feature.GameSessionData;

namespace Feature.CardEffect.Script
{
    /// <summary>
    /// Модификатор целей эффекта. Вкладывается в эффект или в ауру через SerializeReference
    /// (как IGeneratedCardModifier у генератора карт и ICritModifier у крита).
    /// Сначала ReadValue — прочитать своё велью, потом ModifyTargets — применить к целям.
    /// </summary>
    public interface ITargetModifier
    {
        void ReadValue(EffectContext context);

        List<CardAndHealthEntityOwnerData> ModifyTargets(
            List<CardAndHealthEntityOwnerData> targets, EffectContext context);
    }
}
