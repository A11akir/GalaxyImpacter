using System;
using System.Collections.Generic;
using Feature.Card.Script;
using Feature.Hero;

namespace Feature.CardEffect.Script
{
    public enum ClassSource
    {
        FromCasterClass, Manual
    }

    public enum CardTypeFilter
    {
        Any, SpellOnly, MinionOnly 
    }

    public enum RaritySource
    {
        FromDeck, Manual, Any
    }
}