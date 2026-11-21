using System;
using Feature.CardEffect.Script;
using Feature.CombatSystem;
using Feature.GameSessionData;

namespace Feature.PassiveEffect.Script
{
    [Serializable]
    public class BladeMailPassive : PassiveEffectBase, IDamageReaction, ICardContextConsumer
    {
        private CardAndHealthEntityOwnerData _owner;
        private CombatSystem.CombatSystem _combatSystem;
        private bool _isReacting;
        private int _damage;

        public void OnAppliedFromCard(EffectContext context)
        {
            _combatSystem = context.CombatSystem;
            _damage = context.Value();
        }

        public override void Register(CardAndHealthEntityOwnerData owner) => _owner = owner;

        public override void Unregister() { }

        public int Priority => 100;

        public bool ReactToDamage(CardAndHealthEntityOwnerData target, int finalDamage, CardAndHealthEntityOwnerData source)
        {
            if (_isReacting) return false; 
            if (source == null || source == _owner) return false;

            _isReacting = true;
            _combatSystem.DealDamage(source, _damage, _owner, null, DamageType.Normal);
            _isReacting = false;

            return false;
        }

        public override PassiveEffectBase Clone()
        {
            var clone = new BladeMailPassive();
            CopyBaseFields(clone);
            return clone;
        }
    }
}
