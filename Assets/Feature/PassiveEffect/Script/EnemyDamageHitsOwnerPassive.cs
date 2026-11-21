using System;
using Feature.CardEffect.Script;
using Feature.CombatSystem;
using Feature.GameSessionData;

namespace Feature.PassiveEffect.Script
{
    [Serializable]
    public class EnemyDamageHitsOwnerPassive : PassiveEffectBase, ICardContextConsumer
    {
        private CardAndHealthEntityOwnerData _owner;
        private CombatSystem.CombatSystem _combatSystem;
        private GameEventDispatcher _eventDispatcher;
        private GameSessionModel _model;

        public void OnAppliedFromCard(EffectContext context)
        {
            _combatSystem = context.CombatSystem;
            _eventDispatcher = context.EventDispatcher;
            _model = context.GameSessionModel;
        }

        public override void Register(CardAndHealthEntityOwnerData owner)
        {
            _owner = owner;
            _eventDispatcher?.Subscribe<DamageDealtInfo>(OnDamage);
        }

        public override void Unregister()
        {
            _eventDispatcher?.Unsubscribe<DamageDealtInfo>(OnDamage);
        }

        private void OnDamage(DamageDealtInfo info)
        {
            if (info.Source != _owner) return;      // только урон нашего героя
            if (info.Target == _owner) return;      // и не бьём себя повторно
            if (!IsEnemy(info.Target)) return;      // только по противникам

            _combatSystem.DealDamage(_owner, info.Amount, _owner, null, DamageType.Pure);
        }

        private bool IsEnemy(CardAndHealthEntityOwnerData target)
        {
            if (_model == null) return false;

            var targetSide = _model.GetPlayerDataByOwner(target);
            var ownerSide = _model.GetPlayerDataByOwner(_owner);

            return targetSide != null && ownerSide != null && targetSide != ownerSide;
        }

        public override PassiveEffectBase Clone()
        {
            var clone = new EnemyDamageHitsOwnerPassive();
            CopyBaseFields(clone);
            return clone;
        }
    }
}
