using System;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    /// <summary>
    /// Добавляет щит целям (TargetSelection): своё велью = прочность щита.
    /// По умолчанию прочность заменяется, с галочкой — стакается.
    /// </summary>
    [Serializable]
    public class AddShieldEffect : CardEffect
    {
        [SerializeField] private bool _stackShield;

        public override void Execute(EffectContext context)
        {
            int amount = GetValue(context);

            foreach (var target in ResolveTargets(context))
            {
                if (target == null) continue;

                if (_stackShield)
                    target.AddShield(amount);
                else
                    target.SetShield(amount);
            }
        }
    }
}
