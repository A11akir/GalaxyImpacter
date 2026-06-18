using System;
using Feature.GameSessionData;
using Feature.Hero;

namespace Feature.PassiveEffect.Script
{
    [Serializable]
    public class FlatFireDamageBonus : PassiveEffect, IDamageModifier
    {
        private int _bonus;

        public void AddBonus(int amount) => _bonus += amount;


        public override void Register(CardAndHealthEntityOwnerData owner, CombatSystem.CombatSystem combatSystem)
        {
            
        }
        
        public override void Unregister() { }
        public override void OnTurnEnd() => _bonus = 0;

        public int GetDamageBonus(CardStatsData sourceCard)
        {
            if (!sourceCard) return 0;
            if (!sourceCard.Specialization.Contains(AllHeroClass.FireMage)) return 0;
            return _bonus;
        }
    }
}
