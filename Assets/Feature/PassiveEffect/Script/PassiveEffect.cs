using System;
using Feature.GameSessionData;

namespace Feature.PassiveEffect.Script
{
    [Serializable]
    public abstract class PassiveEffect
    {
        public abstract void Register(CardAndHealthEntityOwnerData owner, CombatSystem.CombatSystem combatSystem);
        public abstract void Unregister();
        public virtual void OnTurnEnd() { }
    }
}
