using System;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class HealEffect : CardEffect
    {
        [SerializeField] private int _amount;

        public override void Execute(EffectContext context)
        {
            context.Caster.HealthValue += _amount;
        }
    }
}