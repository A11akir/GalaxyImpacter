using System;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class HealEffect : CardEffect
    {
        public override void Execute(EffectContext context)
        {
            int heal = context.CardData.Values[context.ValueIndex];

            // через HealthReactionSystem — лечение могут запретить пассивки (IHealReaction)
            context.CombatSystem.Heal(context.Target, heal);
        }
    }
}
