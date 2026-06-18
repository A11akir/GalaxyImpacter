using System;
using Feature.GameSessionData;
using UnityEngine;
using UnityEngine.UI;

namespace Feature.PassiveEffect.Script
{
    [Serializable]
    public abstract class PassiveEffect
    {
        public abstract void Register(CardAndHealthEntityOwnerData owner, CombatSystem.CombatSystem combatSystem);
        public abstract void Unregister();
        public virtual void OnTurnEnd() { }
        public Image Icon { get; set; }
        public string Description { get; set; }
    }
}