using System;
using Feature.GameSessionData;
using UnityEngine;

namespace Feature.PassiveEffect.Script
{
    [Serializable]
    public abstract class PassiveEffect
    {
        [SerializeField] protected PassiveEffectConfig Config;
        [SerializeField] protected DurationType Duration = DurationType.Permanent;

        public Sprite Icon => Config?.Icon;
        public string GetDescription(int value) =>
            Config != null ? string.Format(Config.Description, value) : "";

        public void SetConfig(PassiveEffectConfig config) => Config = config;

        public abstract void Register(CardAndHealthEntityOwnerData owner, CombatSystem.CombatSystem combatSystem);
        public abstract void Unregister();

        public bool TickTurnEnd()
        {
            OnTurnTick();
            return Duration == DurationType.UntilTurnEnd;
        }

        protected virtual void OnTurnTick() { }

        public abstract PassiveEffect Clone();
    }
}