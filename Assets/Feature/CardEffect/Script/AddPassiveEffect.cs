// AddPassiveEffect.cs
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
            var passive = ResolvePassive(ctx);

            if (passive is ICardContextConsumer consumer)
                consumer.OnAppliedFromCard(ctx);
        }

        private PassiveEffectBase ResolvePassive(EffectContext ctx)
        {
            if (_passiveTemplate is not IStackablePassive)
                return ctx.Caster.PassiveEffects.Create(_passiveTemplate);

            return ctx.Caster.PassiveEffects.Find(_passiveTemplate.GetType())
                   ?? ctx.Caster.PassiveEffects.Create(_passiveTemplate);
        }
    }
}