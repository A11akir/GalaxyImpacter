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
        private CardAndHealthEntityOwnerData _owner;
        private CombatSystem.CombatSystem _combatSystem;
        private Action<DamageDealtInfo> _handler;

        public override void Register(CardAndHealthEntityOwnerData owner, CombatSystem.CombatSystem combatSystem)
        {
            _owner = owner;
            _combatSystem = combatSystem;
            _handler = OnDamageDealt;
            combatSystem.OnDamageDealt += _handler;
            Debug.Log($"[Watcher] Registered with owner hash={_owner.GetHashCode()}, name={_owner._heroName}");
        }

        public override void Unregister() => _combatSystem.OnDamageDealt -= _handler;

        private void OnDamageDealt(DamageDealtInfo info)
        {
            Debug.Log($"[Watcher] OnDamageDealt: info.Source hash={info.Source?.GetHashCode()}, _owner hash={_owner?.GetHashCode()}, sameRef={info.Source == _owner}");
            if (info.Source != _owner) return;
            if (info.SourceCard == null) return;
            if (!info.SourceCard.Specialization.Contains(AllHeroClass.FireMage)) return;
            
            var bonus = _owner.PassiveEffects.GetPassive<FireDamageBonus>();
            if (bonus == null)
            {
                bonus = new FireDamageBonus { Config = Config }; // тот же конфиг — та же иконка
                _owner.PassiveEffects.AddPassive(bonus, _combatSystem);
            }
            bonus.AddBonus(1);
        }

        public override PassiveEffect Clone() => new FireDamageBonusWatcher { Config = Config };
    }
}
