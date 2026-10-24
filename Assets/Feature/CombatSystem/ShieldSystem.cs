using Feature.GameSessionData;
using UnityEngine;

namespace Feature.CombatSystem
{
    /// <summary>
    /// Щит: гасит min(урон, прочность) за удар, прочность снижается на 1 за любой
    /// удар. Pure-урон проходит мимо щита. Возвращает остаток урона.
    /// </summary>
    public class ShieldSystem
    {
        public int TryBlock(CardAndHealthEntityOwnerData target, int damage, DamageType type)
        {
            if (target == null || damage <= 0) return damage;
            if (type == DamageType.Pure) return damage;
            if (target.ShieldValue <= 0) return damage;

            // Deadly — удар мгновенной смерти: одна прочность щита гасит его целиком.
            if (type == DamageType.Deadly)
            {
                target.ShieldValue -= 1;
                return 0;
            }

            int blocked = Mathf.Min(damage, target.ShieldValue);
            target.ShieldValue -= 1;

            return damage - blocked;
        }
    }
}
