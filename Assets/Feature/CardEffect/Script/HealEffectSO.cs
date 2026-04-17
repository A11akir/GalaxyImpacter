using UnityEngine;

namespace Feature.CardEffect.Script
{
    [CreateAssetMenu(menuName = "Effects/Heal", fileName = "HealEffect")]
    public class HealEffectSO : CardEffectSO
    {
        [SerializeField] private int _amount;

        public override void Execute(EffectContext context)
        {
            context.Caster.HealthValue += _amount;
        }
    }
}