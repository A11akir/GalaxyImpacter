using UnityEngine;

namespace Feature.CardEffect.Script
{
    [CreateAssetMenu(menuName = "Effects/DrawCards", fileName = "DrawCardsEffect")]
    public class DrawCardsEffectSO : CardEffectSO
    {
        
        public override void Execute(EffectContext context)
        {
            for (int i = 0; i < context.CardData.Values[context.ValueIndex]; i++)
            {
                context.Caster.DrawCardFromDeck();
            }
                
        }
    }
}