using System;
using System.Collections.Generic;
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
        private GameSessionPlayerData CasterSide;

        public DeathRattlePassive() => Duration = DurationType.Permanent;

        public void OnAppliedFromCard(EffectContext context)
        {
            GameSessionModel = context.GameSessionModel;
            BattlefieldSystem = context.BattlefieldSystem;
            CombatSystem = context.CombatSystem;
            CardData = context.CardData;
            CardPoolPickSystem = context.CardPoolPickSystem;
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
            };

            CardEffect.ExecuteAll(innerContext, Effects);
        }

        public override PassiveEffectBase Clone() =>
            new DeathRattlePassive
            {
                Effects = Effects,
                Config = Config,
                UseSourceCardAsDescription = UseSourceCardAsDescription
            };
    }
}
