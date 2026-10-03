using System.Collections.Generic;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using Feature.PassiveEffect.Script;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    public abstract class TurnTriggerEffectPassiveBase : PassiveEffect.Script.PassiveEffectBase, ICardContextConsumer
    {
        [SerializeReference] protected List<CardEffect> Effects = new();

        protected CardAndHealthEntityOwnerData Caster;
        protected CardAndHealthEntityOwnerData Target;
        protected GameSessionModel GameSessionModel;
        protected Battlefield.Script.BattlefieldSystem BattlefieldSystem;
        protected CombatSystem.CombatSystem CombatSystem;
        protected CardStatsData CardData;
        protected CardPoolPickSystem CardPoolPickSystem;

        public abstract TurnTriggerTiming Timing { get; }

        public void OnAppliedFromCard(EffectContext context)
        {
            Caster = context.Caster;
            Target = context.Target;
            GameSessionModel = context.GameSessionModel;
            BattlefieldSystem = context.BattlefieldSystem;
            CombatSystem = context.CombatSystem;
            CardData = context.CardData;
            CardPoolPickSystem = context.CardPoolPickSystem;

            context.TurnEffectQueue?.Enqueue(this);
        }

        public override void Register(CardAndHealthEntityOwnerData owner)
        {
        }

        public override void Unregister()
        {
        }

        public void TriggerEffects()
        {
            var innerContext = new EffectContext
            {
                Caster = Caster,
                Target = Target,
                GameSessionModel = GameSessionModel,
                BattlefieldSystem = BattlefieldSystem,
                CombatSystem = CombatSystem,
                CardData = (SpellCardData)CardData,
                CurrentEffectsList = Effects,
                CardPoolPickSystem = CardPoolPickSystem,
            };

            for (int i = 0; i < Effects.Count; i++)
            {
                innerContext.ValueIndex = i;
                Effects[i].Execute(innerContext);
            }
        }
    }
}

public enum TurnTriggerTiming
{
    TurnStart,
    TurnEnd
}