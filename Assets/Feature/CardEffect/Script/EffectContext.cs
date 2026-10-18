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
        // Сторона кастера. Заполняется, когда кастера уже нет в списках владельца
        // (например, deathrattle после удаления существа с доски).
        public GameSessionPlayerData CasterSide;
        // Слот кастера на доске (-1 = нет/неизвестен). Нужен summon'у, чтобы встать на место жертвы.
        public int CasterSlot = -1;
        public CardAndHealthEntityOwnerData Target;
        public List<CardAndHealthEntityOwnerData> Targets;
        public GameSessionModel GameSessionModel;
        public BattlefieldSystem BattlefieldSystem;
        public CombatSystem.CombatSystem CombatSystem;
        public SpellCardData CardData;
        public CardStatsData TargetCard;
        public int ValueIndex;
        
        public int Value(int offset = 0) => CardData.Values[ValueIndex + offset];

        public CardPoolPickSystem CardPoolPickSystem { get; set; }
        public List<CardEffect> CurrentEffectsList { get; set; }
        
        public HashSet<CardAndHealthEntityOwnerData> UsedTargets { get; set; }

        public TurnTriggerEffectQueue TurnEffectQueue;
    }
}