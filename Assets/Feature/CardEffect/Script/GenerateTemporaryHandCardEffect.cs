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
            Debug.Log("GenerateTemporaryHandCardEffect Execute");
            int count = ctx.CardData.Values[ctx.ValueIndex];
            for (int i = 0; i < count; i++)
            {
                var template = ctx.CardPoolPickSystem.Pick(_query, ctx);
                if (!template) return;

                var card = Object.Instantiate(template);
                card.id = Guid.NewGuid().ToString();
                Debug.Log(card.name);
                ctx.Caster.AddCardToHand(card, ctx.Caster.CountCardsInHand);
            }  
        }
    }
}