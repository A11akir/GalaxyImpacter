using UnityEngine;

namespace Feature.CardEffect.Script
{
    [CreateAssetMenu(menuName = "Effects/GenerateTemporaryHandCards", fileName = "GenerateTemporaryHandCardEffect")]
    public class GenerateTemporaryHandCardEffectSO : CardEffectSO
    {
        public override void Execute(EffectContext context)
        {
            for (int i = 0; i < context.CardData.Values[context.ValueIndex]; i++)
            {
                /*context.Caster.AddCardToHand(CardStatsData data, context.Caster.CountCardsInHand);*/
            }
                
        }
    }
}