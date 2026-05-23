using System.Collections.Generic;

namespace Feature.GoogleSheets
{
    public interface ISpellStatsData
    {
        string Name { get; set; }
        int Cost { get; set; }
        List<int> Values { get; set; }
        string Description { get; set; }
        string Rarity { get; set; }
        List<string> Specialization { get; set; }
        int Level { get; set; }
        string Type { get; set; }
    }
}