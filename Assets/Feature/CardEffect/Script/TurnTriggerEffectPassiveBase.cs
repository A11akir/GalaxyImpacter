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
        protected GameSessionPlayerData CasterSide;

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

            // Сторона владельца пассивки — на случай, если он умрёт до триггера.
            CasterSide = context.GameSessionModel.GetPlayerDataByOwner(context.Target);
        }

        public override void Register(CardAndHealthEntityOwnerData owner)
        {
        }

        public override void Unregister()
        {
        }

        public void TriggerEffects()
        {
            // Слот владельца пассивки считаем ДО выполнения эффектов:
            // DieEffect может убить владельца, после чего поиск по списку вернёт -1.
            var side = CasterSide ?? GameSessionModel.GetPlayerDataByOwner(Target);
            int ownerSlot = -1;
            if (side != null && Target != null)
                ownerSlot = side.CardsInBoardList.FindIndex(c => c != null && c.id == Target.CardId);

            var innerContext = new EffectContext
            {
                Caster = Caster,
                Target = Target,
                CasterSide = CasterSide,
                CasterSlot = ownerSlot,
                GameSessionModel = GameSessionModel,
                BattlefieldSystem = BattlefieldSystem,
                CombatSystem = CombatSystem,
                CardData = (SpellCardData)CardData,
                CurrentEffectsList = Effects,
                CardPoolPickSystem = CardPoolPickSystem,
            };

            CardEffect.ExecuteAll(innerContext, Effects);
        }
    }
}

public enum TurnTriggerTiming
{
    TurnStart,
    TurnEnd
}