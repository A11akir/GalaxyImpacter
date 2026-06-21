// EffectContext.cs — добавить поле

using System.Collections.Generic;
using Feature.Battlefield.Script;
using Feature.CardEffect.Script;
using Feature.CombatSystem;
using Feature.GameSessionData;
using Feature.GoogleSheets;

public class EffectContext
{
    public CardAndHealthEntityOwnerData Caster;
    public CardAndHealthEntityOwnerData Target;
    public List<CardAndHealthEntityOwnerData> Targets;
    public GameSessionModel GameSessionModel;
    public BattlefieldSystem BattlefieldSystem;
    public CombatSystem CombatSystem;
    public SpellCardData CardData;
    public int ValueIndex;
    public CardPoolPickSystem CardPoolPickSystem { get; set; }
}