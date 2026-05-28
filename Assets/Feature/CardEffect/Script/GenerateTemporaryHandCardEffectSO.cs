using UnityEngine;

namespace Feature.CardEffect.Script
{
    [CreateAssetMenu(menuName = "Effects/GenerateTemporaryHandCards", fileName = "GenerateTemporaryHandCardEffect")]
    public class GenerateTemporaryHandCardEffectSO : CardEffectSO
    {
        [SerializeField] private CardPickQuery _query;
        
        public override void Execute(EffectContext ctx)
        {
            int count = ctx.CardData.Values[ctx.ValueIndex];
            for (int i = 0; i < count; i++)
            {
                var card = ctx.CardPoolPickSystem.Pick(_query, ctx);
                if (card != null)
                    ctx.Caster.AddCardToHand(card, ctx.Caster.CountCardsInHand);
            }  
        }
    }
}