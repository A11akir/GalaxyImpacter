using System;
using Feature.GameSessionData;
using UnityEngine;
using UnityEngine.UI;

namespace Feature.PassiveEffect.Script
{
    [Serializable]
    public abstract class PassiveEffect
    {
        [SerializeField] protected internal PassiveEffectConfig Config;

        public Sprite Icon => Config?.Icon;

        public string GetDescription(int value) =>
            Config != null ? string.Format(Config.Description, value) : "";

        public abstract void Register(CardAndHealthEntityOwnerData owner, CombatSystem.CombatSystem combatSystem);
        public abstract void Unregister();

        public virtual void OnTurnEnd()
        {
        }

        public abstract PassiveEffect Clone();
    }
}
