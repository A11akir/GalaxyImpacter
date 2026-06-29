// AddPassiveEffect.cs
using System;
using Feature.GameSessionData;
using Feature.GoogleSheets;
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
                var existing = _passiveTemplate is IStackablePassive
                    ? target.PassiveEffects.Find(_passiveTemplate.GetType())
                    : null;

                bool isNew = existing == null;
                var passive = existing ?? _passiveTemplate.Clone();

                passive.SourceCard = (SpellCardData)ctx.CardData;

                if (passive is ICardContextConsumer consumer)
                    consumer.OnAppliedFromCard(ctx); // ← AddBonus/Value меняется ЗДЕСЬ, до Add

                if (isNew)
                    target.PassiveEffects.Add(passive); // ← добавляем в список ПОСЛЕ, когда Value уже актуален
            }
        }
    }
}