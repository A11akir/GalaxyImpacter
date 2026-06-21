// AddPassiveEffect.cs — нужно вызвать Setup, если шаблон оказался TurnEndEffectPassive
using System;
using Feature.PassiveEffect.Script;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class AddPassiveEffect : CardEffect
    {
        [SerializeReference] private PassiveEffectBase _passiveTemplate;

        public override void Execute(EffectContext ctx)
        {
            var passive = ctx.Caster.PassiveEffects.Find(_passiveTemplate.GetType());

            if (passive == null)
            {
                passive = ctx.Caster.PassiveEffects.Create(_passiveTemplate);

                if (passive is TurnEndEffectPassive turnEndPassive)
                    turnEndPassive.Setup(ctx);
            }

            if (passive is IStackablePassive stackable)
                stackable.AddBonus(ctx.CardData.Values[ctx.ValueIndex]);
        }
    }
}