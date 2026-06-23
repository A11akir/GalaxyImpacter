using System;
using Feature.GameSessionData;
using Feature.PassiveEffect;
using Feature.PassiveEffect.Script;
using UnityEngine;

namespace Feature.CombatSystem
{
    public class CombatSystem
    {
        private readonly GameEventDispatcher _eventDispatcher;

        public CombatSystem(GameEventDispatcher eventDispatcher)
        {
            _eventDispatcher = eventDispatcher;
        }
        
        public void TakePureDamage(
            CardAndHealthEntityOwnerData target,
            int damage,
            CardAndHealthEntityOwnerData source,
            CardStatsData sourceCard = null)
        {
            if (target == null) return;
            
            target.LastDamageSource = source;

            target.HealthValue -= damage;
            
            var info = new DamageDealtInfo { Source = source, Target = target, SourceCard = sourceCard, Amount = damage };
            _eventDispatcher.Notify(source, info); 
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
            
            var info = new DamageDealtInfo { Source = source, Target = target, SourceCard = sourceCard, Amount = damage };
            _eventDispatcher.Notify(source, info); 
        }
    }
}
