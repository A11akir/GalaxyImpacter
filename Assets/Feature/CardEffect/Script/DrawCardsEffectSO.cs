using UnityEngine;

namespace Feature.CardEffect.Script
{
    [CreateAssetMenu(menuName = "Effects/DrawCards", fileName = "DrawCardsEffect")]
    public class DrawCardsEffectSO : CardEffectSO
    {
        [SerializeField] private int _count = 1;

        public override void Execute(EffectContext context)
        {
            int count = context.CardData.Values[context.ValueIndex];
            for (int i = 0; i < count+1; i++)
                context.Caster.DrawCardFromDeck();
        }
    }
}