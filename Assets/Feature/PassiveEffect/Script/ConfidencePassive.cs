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
    /// Держит в списке владельца два баффа: шанса (от щита) и урона (от полученного урона).
    /// </summary>
    [Serializable]
    public class ConfidencePassive : PassiveEffectBase, ICardContextConsumer, IDamageReaction
    {
        [SerializeField] private PassiveEffectConfig _chanceConfig;  // CritChanceBuffConfig
        [SerializeField] private PassiveEffectConfig _damageConfig;  // CritDamageBuffConfig

        private CardAndHealthEntityOwnerData _owner;

        // Бафф шанса: его значение = текущая прочность щита.
        private CritChanceBuff _shieldChanceBuff;

        // Бафф урона: его значение = полученный за ход урон.
        private CritDamageBuff _takenDamageBuff;

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
                UpdateShieldChanceBuff(_lastShield);

            owner.Shield
                .Subscribe(value =>
                {
                    int delta = value - _lastShield;
                    _lastShield = value;
                    UpdateShieldChanceBuff(delta);
                })
                .AddTo(_disposables);
        }

        public override void Unregister()
        {
            _disposables.Dispose();
            RemoveBuff(ref _shieldChanceBuff);
            RemoveBuff(ref _takenDamageBuff);
        }

        public bool ReactToDamage(CardAndHealthEntityOwnerData target, int finalDamage, CardAndHealthEntityOwnerData source)
        {
            _damageTakenThisTurn += finalDamage;
            UpdateTakenDamageBuff(finalDamage);
            return false;  
        }

        protected override void OnTurnTick()
        {
            // Старые баффы уберёт Cleanup (они UntilTurnEnd), тут просто забываем ссылки.
            _takenDamageBuff = null;
            _damageTakenThisTurn = 0;

            // Щит пережил ход — вешаем шанс заново на новый ход.
            _shieldChanceBuff = null;
            if (_lastShield > 0)
                UpdateShieldChanceBuff(_lastShield);
        }


        private void UpdateShieldChanceBuff(int delta)
        {
            if (_shieldChanceBuff == null)
            {
                if (delta <= 0) return;

                _shieldChanceBuff = new CritChanceBuff { Duration = DurationType.UntilTurnEnd };
                _shieldChanceBuff.SetConfig(_chanceConfig);
                _owner.PassiveEffects.Add(_shieldChanceBuff);
            }

            _shieldChanceBuff.AddBonus(delta);

            if (_shieldChanceBuff.Value.CurrentValue <= 0)
                RemoveBuff(ref _shieldChanceBuff);
        }


        private void UpdateTakenDamageBuff(int delta)
        {
            if (_takenDamageBuff == null)
            {
                if (delta <= 0) return;

                _takenDamageBuff = new CritDamageBuff { Duration = DurationType.UntilTurnEnd };
                _takenDamageBuff.SetConfig(_damageConfig);
                _owner.PassiveEffects.Add(_takenDamageBuff);
            }

            _takenDamageBuff.AddBonus(delta);

            if (_takenDamageBuff.Value.CurrentValue <= 0)
                RemoveBuff(ref _takenDamageBuff);
        }

        private void RemoveBuff<T>(ref T buff) where T : PassiveEffectBase
        {
            if (buff == null) return;
            _owner.PassiveEffects.Remove(buff);
            buff = null;
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
