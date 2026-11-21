using System;
using Feature.GameSessionData;

namespace Feature.CardEffect.Script
{
    /// <summary>
    /// Выравнивает HP цели на велью процентов от максимального, если урон не смертельный.
    /// Выполняется из DamageTriggerPassive (там же лежит context.Damage).
    /// Велью: слот 0 — процент от максимального HP.
    /// </summary>
    [Serializable]
    public class SetHealthPercentEffect : CardEffect
    {
        public override void Execute(EffectContext context)
        {
            if (context.Damage <= 0) return;

            int percent = context.Value();

            foreach (var target in ResolveTargets(context))
            {
                if (target == null || target.MaxHealth <= 0) continue;

                bool damageIsLethal = target.HealthValue - context.Damage <= 0;
                if (damageIsLethal) continue;

                target.HealthValue = target.MaxHealth * percent / 100;
                context.Damage = 0;   // урон уже учтён новым значением HP
            }
        }
    }
}
