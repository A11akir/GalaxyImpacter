// FireDamageBonusWatcher.cs
using System;
using Feature.CombatSystem;
using Feature.GameSessionData;
using Feature.Hero;
using UnityEngine;

namespace Feature.PassiveEffect.Script
{
    [Serializable]
    public class FireDamageBonusWatcher : PassiveEffectBase
    {
        [SerializeField] private PassiveEffectConfig _bonusConfig;

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
            if (!info.SourceCard) return;
            if (!info.SourceCard.Specialization.Contains(AllHeroClass.FireMage)) return;

            var bonus = _owner.PassiveEffects.Find<FireDamageBonus>();

            if (bonus == null)
            {
                bonus = new FireDamageBonus();
                bonus.SetConfig(_bonusConfig);
                _owner.PassiveEffects.AddPassive(bonus, _combatSystem);
            }
            bonus.AddBonus(1);
        }

        public override PassiveEffectBase Clone() => new FireDamageBonusWatcher { _bonusConfig = _bonusConfig };
    }
}