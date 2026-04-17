using UnityEngine;

namespace Feature.CardEffect.Script
{
    [CreateAssetMenu(menuName = "Effects/Damage", fileName = "DamageEffect")]
    public class DamageEffectSO : CardEffectSO
    {
        public override void Execute(EffectContext context)
        {
            int damage = context.CardData.Values[context.ValueIndex];
            context.Caster.HealthValue -= damage;
            
            Debug.Log(context.Caster._heroName);
            Debug.Log(context.Caster.HealthValue);
        }
    }
}