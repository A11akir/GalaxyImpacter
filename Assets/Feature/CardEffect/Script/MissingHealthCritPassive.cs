using System;
using Feature.GameSessionData;
using Feature.PassiveEffect.Script;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class MissingHealthCritPassive : PassiveEffectBase
    {
        [SerializeField] private PassiveEffectConfig _damageConfig;
        [SerializeReference] private IDynamicCostValueSource _missingHealth = new MissingHealthValueSource();

        private CardAndHealthEntityOwnerData _owner;
        private IDisposable _subscription;
        private int _myBonus;

        public override void Register(CardAndHealthEntityOwnerData owner)
        {
            _owner = owner;

            _subscription = _missingHealth.Subscribe(owner, null, UpdateBuff);
        }

        public override void Unregister()
        {
            _subscription?.Dispose();
            _subscription = null;

            RemoveMyBonus();
        }

        protected override void OnTurnTick()
        {
            if (_myBonus <= 0) return;

            var buff = new CritDamageBuff { Duration = DurationType.UntilTurnEnd };
            buff.SetConfig(_damageConfig);
            buff.AddBonus(_myBonus);
            _owner.PassiveEffects.Add(buff);
        }

        private void UpdateBuff(int missingHealth)
        {
            int delta = missingHealth - _myBonus;
            _myBonus = missingHealth;

            if (delta == 0) return;

            var buff = GetOrCreateBuff(delta);
            if (buff == null) return;

            buff.AddBonus(delta);
            RemoveBuffIfZero(buff);
        }

        private CritDamageBuff GetOrCreateBuff(int delta)
        {
            var buff = _owner.PassiveEffects.Find<CritDamageBuff>();
            if (buff != null) return buff;

            if (delta <= 0) return null;

            buff = new CritDamageBuff { Duration = DurationType.UntilTurnEnd };
            buff.SetConfig(_damageConfig);
            _owner.PassiveEffects.Add(buff);
            return buff;
        }

        private void RemoveMyBonus()
        {
            if (_myBonus <= 0) return;

            var buff = _owner.PassiveEffects.Find<CritDamageBuff>();
            if (buff == null) return;

            buff.AddBonus(-_myBonus);
            RemoveBuffIfZero(buff);
            _myBonus = 0;
        }

        private void RemoveBuffIfZero(CritBuff buff)
        {
            if (buff.Value.CurrentValue > 0) return;

            _owner.PassiveEffects.Remove(buff);
        }

        public override PassiveEffectBase Clone()
        {
            var clone = new MissingHealthCritPassive
            {
                _damageConfig = _damageConfig,
                _missingHealth = _missingHealth
            };
            CopyBaseFields(clone);
            return clone;
        }
    }
}
