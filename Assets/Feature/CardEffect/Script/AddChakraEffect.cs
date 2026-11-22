using UnityEngine;

namespace Feature.CardEffect.Script
{
    public class AddChakraEffect : CardEffect
    {
        [SerializeField] private int _amount;

        public override void Execute(EffectContext context)
        {
            int newChakra = Mathf.Min(
                context.Caster.Chakra + _amount, 
                context.Caster.MaxChakraCountBaseIncrease);
            context.Caster.SetChakra(newChakra);
        }
    }
}