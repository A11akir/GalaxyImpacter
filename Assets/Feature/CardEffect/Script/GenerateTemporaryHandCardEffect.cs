using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class GenerateTemporaryHandCardEffect : CardEffect
    {
        [SerializeField] private CardPickQuery _query;
        
        public override void Execute(EffectContext ctx)
        {
            int count = ctx.CardData.Values[ctx.ValueIndex];
            var targets = ResolveTargets(ctx);

            foreach (var target in targets)
            {
                for (int i = 0; i < count; i++)
                {
                    var template = _query.SpecificCard != null
                        ? _query.SpecificCard
                        : ctx.CardPoolPickSystem.Pick(_query, ctx);

                    if (!template) return;

                    var card = Object.Instantiate(template);
                    card.id = Guid.NewGuid().ToString();
                    target.AddCardToHand(card, target.CountCardsInHand);
                }
            }
        }
    }
}