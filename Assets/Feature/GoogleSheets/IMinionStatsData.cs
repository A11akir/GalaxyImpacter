using System.Collections.Generic;
using Feature.Card.Script;

namespace Feature.GoogleSheets
{
    public interface IMinionStatsData
    {
        string Name { get; set; }
        int Cost { get; set; }
        CardRarity Rarity { get; set; }
        
        TargetType TargetType { get; set; }
        List<string> Specialization { get; set; }
        int Level { get; set; }
        int Health { get; set; }
        int Chakra { get; set; }
        int HandCardCount { get; set; }
        List<SpellCardData> SpellsList { get; set; }
        bool InCollection { get; set; }
    }
}