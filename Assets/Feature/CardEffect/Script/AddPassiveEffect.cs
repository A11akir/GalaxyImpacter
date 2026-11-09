// AddPassiveEffect.cs
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
            var savedTarget = ctx.Target;

            foreach (var target in targets)
            {
                var existing = FindStackable(target, ctx);

                bool isNew = existing == null;
                var passive = existing ?? _passiveTemplate.Clone();

                passive.SourceCard = ctx.CardData;

                if (!isNew && passive is IStackablePassive stackable)
                    stackable.PrepareStack(_passiveTemplate.Duration);

                if (passive is ICardContextConsumer consumer)
                {
                    // Передаём именно resolved-цель: при TargetSelection=Self ctx.Target пустой,
                    // и пассивка не сможет определить своего владельца/сторону.
                    ctx.Target = target;
                    consumer.OnAppliedFromCard(ctx);
                }

                if (isNew)
                    target.PassiveEffects.Add(passive);
            }

            ctx.Target = savedTarget;
        }

        private PassiveEffectBase FindStackable(CardAndHealthEntityOwnerData target, EffectContext ctx)
        {
            if (_passiveTemplate is not IStackablePassive) return null;

            var templateType = _passiveTemplate.GetType();

            foreach (var passive in target.PassiveEffects.ActivePassives.CurrentValue)
            {
                if (passive.GetType() != templateType) continue;

                if (passive is IStackablePassive stackable && stackable.CanStackWith(ctx))
                    return passive;
            }

            return null;
        }
    }
}
