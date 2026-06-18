using System;
using Feature.CombatSystem;
using Feature.GameSessionData;
using Feature.Hero;
using UnityEngine;

namespace Feature.PassiveEffect.Script
{
    [Serializable]
    public class FireDamageBonusPassive : PassiveEffect, IDamageModifier
    {
        private int _bonusAccumulated;
        private CardAndHealthEntityOwnerData _owner;
        private CombatSystem.CombatSystem _combatSystem;
        private Action<DamageDealtInfo> _handler;

        public override void Register(CardAndHealthEntityOwnerData owner, CombatSystem.CombatSystem combatSystem)
        {
            _owner = owner; 
            _combatSystem = combatSystem;
            _handler = OnDamageDealt;
            combatSystem.OnDamageDealt += _handler;
            
            Debug.Log($"Registered {nameof(FireDamageBonusPassive)} for {_owner}");
        }

        public override void Unregister()
        {
            _combatSystem.OnDamageDealt -= _handler;
        }

        public void AddBonus(int amount) => _bonusAccumulated += amount;

        public override void OnTurnEnd() => _bonusAccumulated = 0;

        public int GetDamageBonus(CardStatsData sourceCard)
        {
            if (sourceCard == null) return 0;
            if (!sourceCard.Specialization.Contains(AllHeroClass.FireMage)) return 0;

            Debug.Log(sourceCard.Name);
            return _bonusAccumulated;
        }

        private void OnDamageDealt(DamageDealtInfo info)
        {
            Debug.Log("DamageDealtInfo received in FireDamageBonusPassive");
            if (info.Source != _owner) return;
            if (info.SourceCard == null) return;
            if (!info.SourceCard.Specialization.Contains(AllHeroClass.FireMage)) return;
            _bonusAccumulated++;
            Debug.Log(_bonusAccumulated);
        }
    }
}
