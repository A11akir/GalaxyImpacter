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
            if (_passiveTemplate is IStackablePassive)
            {
                var passive = ctx.Caster.PassiveEffects.Find(_passiveTemplate.GetType());

                if (passive == null)
                {
                    passive = ctx.Caster.PassiveEffects.Create(_passiveTemplate);
                }

                if (passive is IStackablePassive stackable)
                {
                    stackable.AddBonus(ctx.CardData.Values[ctx.ValueIndex]);
                }

                return;
            }

            var newPassive = ctx.Caster.PassiveEffects.Create(_passiveTemplate);

            if (newPassive is TurnEndEffectPassive turnEndPassive)
            {
                turnEndPassive.Setup(ctx);
            }
        }
    }
}