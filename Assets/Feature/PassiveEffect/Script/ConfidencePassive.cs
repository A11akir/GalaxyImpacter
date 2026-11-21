using System;
using Feature.CardEffect.Script;
using Feature.GameSessionData;
using R3;
using UnityEngine;

namespace Feature.PassiveEffect.Script
{
    /// <summary>
    /// Самоуверенность (сила героя): шанс крита = прочность щита,
    /// % урона крита = полученный за ход урон.
    /// Баффы общие: если такой бафф уже висит (например, от карты), значения
    /// добавляются в него, а не создаётся второй эффект.
    /// </summary>
    [Serializable]
    public class ConfidencePassive : PassiveEffectBase, ICardContextConsumer, IDamageReaction
    {
        [SerializeField] private PassiveEffectConfig _chanceConfig;  // CritChanceBuffConfig
        [SerializeField] private PassiveEffectConfig _damageConfig;  // CritDamageBuffConfig

        private CardAndHealthEntityOwnerData _owner;

        private readonly CompositeDisposable _disposables = new();
        private int _lastShield;
        private int _damageTakenThisTurn;

        public int Priority => 0;

        public void OnAppliedFromCard(EffectContext context) { }

        public override void Register(CardAndHealthEntityOwnerData owner)
        {
            _owner = owner;
            _lastShield = owner.ShieldValue;

            if (_lastShield > 0)
                AddChanceFromShield(_lastShield);

            owner.Shield
                .Subscribe(value =>
                {
                    int delta = value - _lastShield;
                    _lastShield = value;
                    AddChanceFromShield(delta);
                })
                .AddTo(_disposables);
        }

        public override void Unregister()
        {
            _disposables.Dispose();

            Subtract<CritChanceBuff>(_lastShield);
            Subtract<CritDamageBuff>(_damageTakenThisTurn);
        }

        public bool ReactToDamage(CardAndHealthEntityOwnerData target, int finalDamage, CardAndHealthEntityOwnerData source)
        {
            _damageTakenThisTurn += finalDamage;
            AddDamageTaken(finalDamage);
            return false;
        }

        protected override void OnTurnTick()
        {
            // Бафф урона живёт до конца хода — его снимает CleanupExpiredPassives.
            _damageTakenThisTurn = 0;
        }

        /// <summary>Шанс крита = текущая прочность щита. Добавляем только дельту.</summary>
        private void AddChanceFromShield(int delta)
        {
            if (delta == 0) return;

            var buff = _owner.PassiveEffects.Find<CritChanceBuff>();

            if (buff == null)
            {
                if (delta <= 0) return;

                buff = new CritChanceBuff { Duration = DurationType.Permanent };
                buff.SetConfig(_chanceConfig);
                _owner.PassiveEffects.Add(buff);
            }

            buff.AddBonus(delta);
            RemoveBuffIfEmpty(buff);
        }

        /// <summary>Урон крита = полученный за ход урон. Добавляем только дельту.</summary>
        private void AddDamageTaken(int delta)
        {
            if (delta <= 0) return;

            var buff = _owner.PassiveEffects.Find<CritDamageBuff>();

            if (buff == null)
            {
                buff = new CritDamageBuff { Duration = DurationType.UntilTurnEnd };
                buff.SetConfig(_damageConfig);
                _owner.PassiveEffects.Add(buff);
            }

            buff.AddBonus(delta);
        }

        private void Subtract<T>(int amount) where T : CritBuff
        {
            if (amount <= 0) return;

            var buff = _owner.PassiveEffects.Find<T>();
            if (buff == null) return;

            buff.AddBonus(-amount);
            RemoveBuffIfEmpty(buff);
        }

        private void RemoveBuffIfEmpty(CritBuff buff)
        {
            if (buff.Value.CurrentValue > 0) return;

            _owner.PassiveEffects.Remove(buff);
        }

        public override PassiveEffectBase Clone()
        {
            var clone = new ConfidencePassive
            {
                _chanceConfig = _chanceConfig,
                _damageConfig = _damageConfig
            };
            CopyBaseFields(clone);
            return clone;
        }
    }
}
