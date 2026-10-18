using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
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
            int count = ctx.Value();
            var targets = ResolveTargets(ctx);

            foreach (var target in targets)
            {
                for (int i = 0; i < count; i++)
                {
                    var card = ctx.CardPoolPickSystem.PickInstance(filter, ctx);
                    if (card == null) return;

                    card.Origin = CardOrigin.Temporary;
                    ApplyModifiers(ctx, card);

                    target.AddCardToHand(card, target.CountCardsInHand);
                }
            }
        }

        private void ApplyModifiers(EffectContext ctx, CardStatsData card)
        {
            for (int m = 0; m < _modifiers.Count; m++)
            {
                var modifier = _modifiers[m];
                if (modifier == null) continue;

                ExecuteInSlot(ctx, 1 + m, c =>
                {
                    c.TargetCard = card;
                    modifier.Execute(c);
                });
            }
        }
    }
}
