using System;
using Feature.GameSessionData;
using UnityEngine;

namespace Feature.CombatSystem
{
    public class CombatSystem
    {
        public event Action<DamageDealtInfo> OnDamageDealt;
        public void TakePureDamage(
            CardAndHealthEntityOwnerData target,
            int damage,
            CardAndHealthEntityOwnerData source,
            CardStatsData sourceCard = null)
        {
            if (target == null) return;
            
            target.LastDamageSource = source;

            target.HealthValue -= damage;
            
            OnDamageDealt?.Invoke(new DamageDealtInfo
            {
                Source = source,
                Target = target,
                SourceCard = sourceCard,
                Amount = damage
            });
        }
        
        public void TakeDamage(
            CardAndHealthEntityOwnerData target,
            int damage,
            CardAndHealthEntityOwnerData source,
            CardStatsData sourceCard = null)
        {
            if (target == null) return;

            int damageLeft = damage;
            
            if (target.ArmorValue > 0)
            {
                int absorbed = Mathf.Min(target.ArmorValue, damageLeft);

                target.ArmorValue -= absorbed;

                damageLeft -= absorbed;
            }
            
            if (damageLeft > 0)
                target.HealthValue -= damageLeft;
            
            target.LastDamageSource = source;
            
            OnDamageDealt?.Invoke(new DamageDealtInfo
            {
                Source = source,
                Target = target,
                SourceCard = sourceCard,
                Amount = damage
            });
        }
    }
}
