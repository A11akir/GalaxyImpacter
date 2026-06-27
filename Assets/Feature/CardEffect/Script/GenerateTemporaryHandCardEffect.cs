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
            Debug.Log($"[GenerateTemporaryHandCardEffect] Execute, ValueIndex={ctx.ValueIndex}, CardData.Values.Count={ctx.CardData.Values.Count}, CardPoolPickSystem={ctx.CardPoolPickSystem != null}");

            int count = ctx.CardData.Values[ctx.ValueIndex];
            Debug.Log($"[GenerateTemporaryHandCardEffect] count={count}");

            for (int i = 0; i < count; i++)
            {
                var template = ctx.CardPoolPickSystem.Pick(_query, ctx);
                Debug.Log($"[GenerateTemporaryHandCardEffect] iteration={i}, template={template?.Name ?? "NULL"}");

                if (!template)
                {
                    Debug.LogWarning("[GenerateTemporaryHandCardEffect] template is null, returning early");
                    return;
                }

                var card = Object.Instantiate(template);
                card.id = Guid.NewGuid().ToString();
                ctx.Caster.AddCardToHand(card, ctx.Caster.CountCardsInHand);
                Debug.Log($"[GenerateTemporaryHandCardEffect] added card={card.Name} to hand, hand count now={ctx.Caster.CountCardsInHand}");
            }
        }
    }
}