using Feature.GameSessionData;
using Feature.PassiveEffect;

namespace Feature.CombatSystem
{
    public class CombatSystem
    {
        private readonly GameSessionModel _gameSessionModel;
        private readonly GameEventDispatcher _eventDispatcher;
        private readonly HealthReactionSystem _healthReactionSystem;

        public CombatSystem(GameEventDispatcher eventDispatcher, GameSessionModel gameSessionModel, HealthReactionSystem healthReactionSystem)
        {
            _eventDispatcher = eventDispatcher;
            _gameSessionModel = gameSessionModel;
            _healthReactionSystem = healthReactionSystem;
        }

        /// <summary>
        /// Наносит урон. Возвращает true, если именно этот урон убил цель.
        /// </summary>
        public bool DealDamage(
            CardAndHealthEntityOwnerData target,
            int damage,
            CardAndHealthEntityOwnerData source,
            CardStatsData sourceCard = null,
            DamageType type = DamageType.Normal)
        {
            if (target == null) return false;
            if (damage <= 0) return false;

            bool isLethal = _healthReactionSystem.ApplyDamage(target, damage, type, IsHero(target), source);

            target.LastDamageSource = source;

            var info = new DamageDealtInfo { Source = source, Target = target, SourceCard = sourceCard, Amount = damage };
            _eventDispatcher.Notify(source, info);

            return isLethal;
        }

        private bool IsHero(CardAndHealthEntityOwnerData target)
        {
            var playerData = _gameSessionModel.GetPlayerDataByOwner(target);
            return playerData != null && target == playerData.MainHeroEntity();
        }
    }
}