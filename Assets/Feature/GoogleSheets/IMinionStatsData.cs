using System.Collections.Generic;
using Feature.Card.Script;
using Feature.Hero;

namespace Feature.GoogleSheets
{
    public interface IMinionStatsData
    {
        string Name { get; set; }
        int Cost { get; set; }
        CardRarity Rarity { get; set; }
        
        TargetType TargetType { get; set; }
        List<AllHeroClass> Specialization { get; set; }
        int Level { get; set; }
        int Health { get; set; }
        int Chakra { get; set; }
        int HandCardCount { get; set; }
        List<SpellCardData> SpellsList { get; set; }
        bool InCollection { get; set; }
        int BaseCost { get; set; }
    }
}