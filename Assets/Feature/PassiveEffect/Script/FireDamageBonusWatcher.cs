// FireDamageBonusWatcher.cs
using System;
using Feature.CombatSystem;
using Feature.GameSessionData;
using Feature.Hero;
using UnityEngine;

namespace Feature.PassiveEffect.Script
{
    [Serializable]
    public class FireDamageBonusWatcher : PassiveEffect
    {
        [SerializeField] private PassiveEffectConfig _bonusConfig; // ← отдельное поле для дочерней пассивки

        private CardAndHealthEntityOwnerData _owner;
        private CombatSystem.CombatSystem _combatSystem;
        private Action<DamageDealtInfo> _handler;

        public override void Register(CardAndHealthEntityOwnerData owner, CombatSystem.CombatSystem combatSystem)
        {
            _owner = owner;
            _combatSystem = combatSystem;
            _handler = OnDamageDealt;
            combatSystem.OnDamageDealt += _handler;
        }

        public override void Unregister() => _combatSystem.OnDamageDealt -= _handler;
        
        private void OnDamageDealt(DamageDealtInfo info)
        {
            if (info.Source != _owner) return;
            if (info.SourceCard == null) return;
            if (!info.SourceCard.Specialization.Contains(AllHeroClass.FireMage)) return;

            var bonus = _owner.PassiveEffects.GetPassive<FireDamageBonus>();
            if (bonus == null)
            {
                bonus = new FireDamageBonus();
                bonus.SetConfig(_bonusConfig);
                Debug.Log($"[Watcher] Bonus created/incremented");
                _owner.PassiveEffects.AddPassive(bonus, _combatSystem);
            }
            bonus.AddBonus(1);
        }

        public override PassiveEffect Clone() => new FireDamageBonusWatcher { _bonusConfig = _bonusConfig };
    }
}