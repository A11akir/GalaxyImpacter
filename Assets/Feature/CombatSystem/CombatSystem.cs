using System;
using Feature.GameSessionData;
using UnityEngine;

namespace Feature.CombatSystem
{
    public class CombatSystem
    {
        public event Action<DamageDealtInfo> OnDamageDealt;

        public void TakeDamage(CardAndHealthEntityOwnerData target, int damage, CardAndHealthEntityOwnerData source, CardStatsData sourceCard = null)
        {
            if (target == null) return;

            target.LastDamageSource = source;
            Debug.Log(target._heroName);
            target.HealthValue -= damage;

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
