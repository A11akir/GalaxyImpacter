using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;
using Feature.Card.Script;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class GenerateTemporaryHandCardEffect : CardEffect
    {
        [FormerlySerializedAs("_query")] [SerializeField] private CardPickFilter filter = new();
        
        [SerializeReference] private List<IGeneratedCardModifier> _modifiers = new();

        public override void Execute(EffectContext ctx)
        {
            int count = ctx.CardData.Values[ctx.ValueIndex];
            var targets = ResolveTargets(ctx);

            foreach (var target in targets)
            {
                for (int i = 0; i < count; i++)
                {
                    var template = ctx.CardPoolPickSystem.Pick(filter, ctx);
                    if (!template) return;

                    var card = Object.Instantiate(template);
                    card.id = Guid.NewGuid().ToString();
                    card.Origin = CardOrigin.Temporary;

                    ApplyModifiers(ctx, card);

                    target.AddCardToHand(card, target.CountCardsInHand);
                }
            }
        }

        private void ApplyModifiers(EffectContext ctx, CardStatsData card)
        {
            int countIndex = ctx.ValueIndex;
            var savedCard = ctx.TargetCard;

            for (int m = 0; m < _modifiers.Count; m++)
            {
                var modifier = _modifiers[m];
                if (modifier == null) continue;

                ctx.ValueIndex = countIndex + 1 + m;
                ctx.TargetCard = card;
                modifier.Execute(ctx);
            }

            ctx.ValueIndex = countIndex;
            ctx.TargetCard = savedCard;
        }
    }
}
