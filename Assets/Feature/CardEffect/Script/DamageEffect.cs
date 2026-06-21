// DamageEffect.cs — теперь работает через общий ResolveTargets, без своей логики выбора цели
using System;
using Feature.PassiveEffect.Script;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class DamageEffect : CardEffect
    {
        public override void Execute(EffectContext context)
        {
            int damage = context.CardData.Values[context.ValueIndex];

            int bonus = 0;
            foreach (var passive in context.Caster.PassiveEffects.ActivePassives.CurrentValue)
                if (passive is IDamageModifier modifier)
                    bonus += modifier.GetDamageBonus(context.CardData);

            var targets = ResolveTargets(context);

            foreach (var target in targets)
                context.CombatSystem.TakeDamage(target, damage + bonus, context.Caster, context.CardData);
        }
    }
}