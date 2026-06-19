
using System;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class AddPassiveEffect : CardEffect
    {
        [SerializeReference] private PassiveEffect.Script.PassiveEffectBase _passiveTemplate;

        public override void Execute(EffectContext ctx)
        {
            var passive = ctx.Caster.PassiveEffects.Find(_passiveTemplate.GetType());

            if (passive == null)
                passive = ctx.Caster.PassiveEffects.Create(_passiveTemplate, ctx.CombatSystem);

            if (passive is IStackablePassive stackable)
                stackable.AddBonus(ctx.CardData.Values[ctx.ValueIndex]);
        }
    }
}