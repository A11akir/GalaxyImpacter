using Feature.GameSessionData;
using Feature.PassiveEffect.Script;
using UnityEngine;

namespace Feature.CombatSystem
{
    public class HealthReactionSystem
    {
        public void ApplyDamage(CardAndHealthEntityOwnerData target, int damage, DamageType type, bool targetIsHero, CardAndHealthEntityOwnerData source)
        {
            int finalDamage = CalculateFinalDamage(target, damage, type, targetIsHero);

            foreach (var passive in target.PassiveEffects.ActivePassives.CurrentValue)
            {
                if (passive is IDamageReaction reaction && reaction.ReactToDamage(target, finalDamage, source))
                    return;
            }

            target.HealthValue -= finalDamage;
        }

        private int CalculateFinalDamage(CardAndHealthEntityOwnerData target, int damage, DamageType type, bool targetIsHero)
        {
            switch (type)
            {
                case DamageType.Pure:
                    return damage;

                case DamageType.Deadly:
                    if (!targetIsHero)
                    {
                        target.HealthValue = 0;
                        return 0;
                    }
                    return ApplyArmor(target, damage);

                case DamageType.Normal:
                default:
                    return ApplyArmor(target, damage);
            }
        }

        private int ApplyArmor(CardAndHealthEntityOwnerData target, int damage)
        {
            int damageLeft = damage;

            if (target.ArmorValue > 0)
            {
                int absorbed = Mathf.Min(target.ArmorValue, damageLeft);
                target.ArmorValue -= absorbed;
                damageLeft -= absorbed;
            }

            return damageLeft;
        }
    }
}