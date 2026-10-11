using System;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class GenerateTemporaryHandCardEffect : CardEffect
    {
        [FormerlySerializedAs("_query")] [SerializeField] private CardPickFilter filter = new();
        
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
                    target.AddCardToHand(card, target.CountCardsInHand);
                }
            }
        }
    }
}