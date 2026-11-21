using System;
using System.Collections.Generic;
using Feature.Card.Script;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using Feature.PassiveEffect.Script;
using UnityEngine;

namespace Feature.CardEffect.Script
{

    [Serializable]
    public class DeathRattlePassive : PassiveEffectBase, ICardContextConsumer
    {
        [SerializeReference] private List<CardEffect> Effects = new();

        private GameSessionModel GameSessionModel;
        private Battlefield.Script.BattlefieldSystem BattlefieldSystem;
        private CombatSystem.CombatSystem CombatSystem;
        private CardStatsData CardData;
        private CardPoolPickSystem CardPoolPickSystem;
        private CritSystem CritSystem;
        private GameSessionPlayerData CasterSide;
        private CalculateCardCostSystem CostSystem;

        public DeathRattlePassive() => Duration = DurationType.Permanent;

        public void OnAppliedFromCard(EffectContext context)
        {
            GameSessionModel = context.GameSessionModel;
            BattlefieldSystem = context.BattlefieldSystem;
            CombatSystem = context.CombatSystem;
            CardData = context.CardData;
            CardPoolPickSystem = context.CardPoolPickSystem;
            CritSystem = context.CritSystem;
            CostSystem = context.CostSystem;
            CasterSide = context.GameSessionModel.GetPlayerDataByOwner(context.Target);
        }

        public override void Register(CardAndHealthEntityOwnerData owner) { }
        public override void Unregister() { }
        public void Trigger(CardAndHealthEntityOwnerData victim, CardAndHealthEntityOwnerData killer, int victimSlot)
        {
            var innerContext = new EffectContext
            {
                Caster = victim,
                CasterSide = CasterSide,
                CasterSlot = victimSlot,
                Target = killer,
                GameSessionModel = GameSessionModel,
                BattlefieldSystem = BattlefieldSystem,
                CombatSystem = CombatSystem,
                CardData = (SpellCardData)CardData,
                CurrentEffectsList = Effects,
                CardPoolPickSystem = CardPoolPickSystem,
                CritSystem = CritSystem,
                CostSystem = CostSystem,
            };

            CardEffect.ExecuteAll(innerContext, Effects);
        }

        public override PassiveEffectBase Clone()
        {
            var clone = new DeathRattlePassive { Effects = Effects };
            CopyBaseFields(clone);
            return clone;
        }
    }
}
