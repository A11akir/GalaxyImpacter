using System;

namespace Feature.CardEffect.Script
{
    /// <summary>
    /// Убивает цель напрямую — без урона, брони и модификаторов.
    /// Смерть триггерится подпиской на Health (как обычная смерть).
    /// </summary>
    [Serializable]
    public class DieEffect : CardEffect
    {
        public override void Execute(EffectContext context)
        {
            foreach (var target in ResolveTargets(context))
            {
                if (target == null) continue;

                target.LastDamageSource = context.Caster;
                target.HealthValue = 0;
            }
        }
    }
}
