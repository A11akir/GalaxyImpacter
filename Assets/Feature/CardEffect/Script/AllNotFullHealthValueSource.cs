using System;
using System.Linq;

namespace Feature.CardEffect.Script
{
    /// <summary>Значение = сколько сущностей (обе стороны) имеют неполное здоровье.</summary>
    [Serializable]
    public class AllNotFullHealthValueSource : IEffectValueSource
    {
        public int GetValue(EffectContext context) =>
            context.GameSessionModel.GetAllEntityOwners().Count(e => e.HealthValue < e.MaxHealth);
    }
}
