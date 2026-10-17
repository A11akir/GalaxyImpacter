using System.Collections.Generic;
using Feature.Battlefield.Script;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using Feature.PassiveEffect.Script;

namespace Feature.CardEffect.Script
{
    public class EffectContext
    {
        public CardAndHealthEntityOwnerData Caster;
        public CardAndHealthEntityOwnerData Target;
        public List<CardAndHealthEntityOwnerData> Targets;
        public GameSessionModel GameSessionModel;
        public BattlefieldSystem BattlefieldSystem;
        public CombatSystem.CombatSystem CombatSystem;
        public SpellCardData CardData;
        public CardStatsData TargetCard;
        public int ValueIndex;

        /// <summary>
        /// Значение из Values по слоту ValueIndex + offset.
        /// Вне диапазона — 0, чтобы неполный конфиг не бросал IndexOutOfRange.
        /// </summary>
        public int Value(int offset = 0)
        {
            var values = CardData != null ? CardData.Values : null;
            int index = ValueIndex + offset;

            if (values == null || index < 0 || index >= values.Count)
                return 0;

            return values[index];
        }

        public CardPoolPickSystem CardPoolPickSystem { get; set; }
        public List<CardEffect> CurrentEffectsList { get; set; }
        
        public HashSet<CardAndHealthEntityOwnerData> UsedTargets { get; set; }

        public TurnTriggerEffectQueue TurnEffectQueue;
    }
}