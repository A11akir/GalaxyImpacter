using System;

namespace Feature.CardEffect.Script
{
    /// <summary>
    /// Добавляет щит целям (TargetSelection): своё велью = прочность щита.
    /// </summary>
    [Serializable]
    public class AddShieldEffect : CardEffect
    {
        public override void Execute(EffectContext context)
        {
            int amount = context.Value();

            foreach (var target in ResolveTargets(context))
            {
                if (target == null) continue;
                target.AddShield(amount);
            }
        }
    }
}
