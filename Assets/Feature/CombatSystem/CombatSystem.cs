using Feature.GameSessionData;
using Feature.PassiveEffect;
using UnityEngine;

namespace Feature.CombatSystem
{
    public class CombatSystem
    {
        private readonly GameSessionModel _gameSessionModel;
        private readonly GameEventDispatcher _eventDispatcher;

        public CombatSystem(GameEventDispatcher eventDispatcher, GameSessionModel gameSessionModel)
        {
            _eventDispatcher = eventDispatcher;
            _gameSessionModel = gameSessionModel;
        }

        public void DealDamage(
            CardAndHealthEntityOwnerData target,
            int damage,
            CardAndHealthEntityOwnerData source,
            CardStatsData sourceCard = null,
            DamageType type = DamageType.Normal)
        {
            if (target == null) return;

            switch (type)
            {
                case DamageType.Normal:
                    ApplyNormalDamage(target, damage);
                    break;

                case DamageType.Pure:
                    ApplyPureDamage(target, damage);
                    break;

                case DamageType.Deadly:
                    ApplyDeadlyDamage(target, damage);
                    break;
            }

            target.LastDamageSource = source;

            var info = new DamageDealtInfo { Source = source, Target = target, SourceCard = sourceCard, Amount = damage };
            _eventDispatcher.Notify(source, info);
        }

        private void ApplyNormalDamage(CardAndHealthEntityOwnerData target, int damage)
        {
            int damageLeft = damage;

            if (target.ArmorValue > 0)
            {
                int absorbed = Mathf.Min(target.ArmorValue, damageLeft);
                target.ArmorValue -= absorbed;
                damageLeft -= absorbed;
            }

            if (damageLeft > 0)
                target.HealthValue -= damageLeft;
        }

        private void ApplyPureDamage(CardAndHealthEntityOwnerData target, int damage)
        {
            target.HealthValue -= damage;
        }

        private void ApplyDeadlyDamage(CardAndHealthEntityOwnerData target, int damage)
        {
            bool isMinion = !IsHero(target);

            if (isMinion)
            {
                target.HealthValue = 0;
            }
            else
            {
                ApplyNormalDamage(target, damage); 
            }
        }
        
        private bool IsHero(CardAndHealthEntityOwnerData target)
        {
            var playerData = _gameSessionModel.GetPlayerDataByOwner(target);
            return playerData != null && target == playerData.MainHeroEntity();
        }
    }
}