using UnityEngine;

namespace Feature.CardEffect.Script
{
    [CreateAssetMenu(menuName = "Effects/AddChakra", fileName = "AddChakraEffect")]
    public class AddChakraEffectSO : CardEffectSO
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