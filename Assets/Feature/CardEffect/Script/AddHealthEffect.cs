using System;

namespace Feature.CardEffect.Script
{
    /// <summary>
    /// Добавляет здоровье: сначала +к максимуму, затем столько же к текущему.
    /// Не лечит до полного — оба значения растут на одну величину.
    /// </summary>
    [Serializable]
    public class AddHealthEffect : CardEffect
    {
        public override void Execute(EffectContext context)
        {
            int amount = GetValue(context);

            foreach (var target in ResolveTargets(context))
            {
                if (target == null) continue;

                target.MaxHealth += amount;
                target.HealthValue += amount;
            }
        }
    }
}
