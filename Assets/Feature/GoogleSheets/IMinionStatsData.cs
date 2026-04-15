using System.Collections.Generic;

namespace Feature.GoogleSheets
{
    public interface IMinionStatsData
    {
        string Name { get; set; }
        int Cost { get; set; }
        string Rarity { get; set; }
        List<string> Specialization { get; set; }
        int Level { get; set; }
        int Health { get; set; }
        int Chakra { get; set; }
        int HandCardCount { get; set; }
        List<SpellStatsConfig> SpellsList { get; set; }
    }
}