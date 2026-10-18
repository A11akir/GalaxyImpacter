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
            var targets = ResolveTargets(ctx);
            var savedTarget = ctx.Target;

            foreach (var target in targets)
            {
                var existing = _passiveTemplate is IStackablePassive
                    ? target.PassiveEffects.Find(_passiveTemplate.GetType())
                    : null;

                bool isNew = existing == null;
                var passive = existing ?? _passiveTemplate.Clone();

                passive.SourceCard = ctx.CardData;

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
    }
}