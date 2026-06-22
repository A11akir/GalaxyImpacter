using System;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class BarrageRepeatEffect : CardEffect
    {
        public override void Execute(EffectContext context)
        {
            var damageEffect = new DamageEffect();

            int repeatCount = damageEffect.CalculateDamage(context);
            
            Debug.Log($"BarrageRepeatEffect: Calculated repeat count = {repeatCount}");

            for (int i = 0; i < repeatCount; i++)
            {
                var target = ResolveTargets(context);
                
                var innerContext = new EffectContext
                {
                    Caster = context.Caster,
                    Target = target[0],
                    GameSessionModel = context.GameSessionModel,
                    BattlefieldSystem = context.BattlefieldSystem,
                    CombatSystem = context.CombatSystem,
                    CardData = context.CardData,
                    CurrentEffectsList = context.CurrentEffectsList,
                    ValueIndex = context.ValueIndex
                };

                
                damageEffect.Execute(innerContext);
            }
        }
    }
}