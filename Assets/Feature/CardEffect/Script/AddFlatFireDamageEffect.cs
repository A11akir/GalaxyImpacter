using System;
using Feature.PassiveEffect.Script;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class AddFlatFireDamageEffect : CardEffect
    {
        public override void Execute(EffectContext context)
        {
            int amount = context.CardData.Values[context.ValueIndex];
            
            foreach (var passive in context.Caster.PassiveEffects.ActivePassives.CurrentValue)
                if (passive is FlatFireDamageBonus flatBonus)
                {
                    flatBonus.AddBonus(amount);
                    return;
                }
            
            var newBonus = new FlatFireDamageBonus();
            context.Caster.PassiveEffects.AddPassive(newBonus, context.CombatSystem);
            newBonus.AddBonus(amount);
        }
    }
}
