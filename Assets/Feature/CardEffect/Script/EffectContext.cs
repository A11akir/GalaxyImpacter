using System.Collections.Generic;
using Feature.Battlefield.Script;
using Feature.GameSessionData;
using Feature.GoogleSheets;

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
        public int ValueIndex;
        public CardPoolPickSystem CardPoolPickSystem { get; set; }
        public List<CardEffect> CurrentEffectsList { get; set; }
    }
}