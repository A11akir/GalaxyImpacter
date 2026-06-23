
using System;
using Feature.GameSessionData;
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
            var targets = ResolveTargets(ctx);

            foreach (var target in targets)
            {
                var passive = ResolvePassive(ctx, target);

                if (passive is ICardContextConsumer consumer)
                    consumer.OnAppliedFromCard(ctx);
            }
        }

        private PassiveEffectBase ResolvePassive(EffectContext ctx, CardAndHealthEntityOwnerData owner)
        {
            if (_passiveTemplate is not IStackablePassive)
                return owner.PassiveEffects.Create(_passiveTemplate);

            return owner.PassiveEffects.Find(_passiveTemplate.GetType())
                   ?? owner.PassiveEffects.Create(_passiveTemplate);
        }
    }
}