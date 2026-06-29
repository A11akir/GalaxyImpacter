using System;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using UnityEngine;

namespace Feature.PassiveEffect.Script
{
    [Serializable]
    public abstract class PassiveEffectBase
    {
        [SerializeField] protected PassiveEffectConfig Config;
        [SerializeField] public DurationType Duration;
        public SpellCardData SourceCard { get; set; }
        public Sprite Icon => Config?.Icon;
        public string GetDescription(int value)
        {
            return Config ? string.Format(Config.Description, value) : "";
        }

        public void SetConfig(PassiveEffectConfig config) => Config = config;
        
        public virtual int GetDisplayValue(int duplicateCount) => duplicateCount;
        
        public abstract void Register(
            CardAndHealthEntityOwnerData owner);
        
        public abstract void Unregister();

        public bool TickTurnEnd()
        {
            OnTurnTick();
            return Duration == DurationType.UntilTurnEnd;
        }

        protected virtual void OnTurnTick() { }

        public abstract PassiveEffectBase Clone();
    }
}