using System;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using UnityEngine;
using UnityEngine.Serialization;

namespace Feature.PassiveEffect.Script
{
    [Serializable]
    public abstract class PassiveEffectBase
    {
        [SerializeField] protected PassiveEffectConfig Config; 
        [SerializeField] protected bool UseSourceCardAsDescription;
        [SerializeField] public DurationType Duration;

        /// <summary>Не создавать вьюху эффекта — работает только под капотом.</summary>
        [SerializeField] public bool HideView;

        public SpellCardData SourceCard { get; set; }

        public virtual Sprite Icon =>
            UseSourceCardAsDescription && SourceCard != null
                ? SourceCard.IconImage
                : Config?.Icon;

        public virtual string GetDescription(int value)
        {
            if (UseSourceCardAsDescription && SourceCard != null)
                return string.Format(SourceCard.Description, value);

            return Config ? string.Format(Config.Description, value) : "";
        }

        public void SetConfig(PassiveEffectConfig config) => Config = config;
        
        protected void CopyBaseFields(PassiveEffectBase clone)
        {
            clone.Config = Config;
            clone.Duration = Duration;
            clone.UseSourceCardAsDescription = UseSourceCardAsDescription;
            clone.HideView = HideView;
        }

        public abstract void Register(CardAndHealthEntityOwnerData owner);
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