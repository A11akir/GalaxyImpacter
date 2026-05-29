using System.Collections.Generic;
using Feature.Card.Script;
using Feature.Hero;

namespace Feature.GoogleSheets
{
    public interface ISpellStatsData
    {
        string Name { get; set; }
        int Cost { get; set; }
        List<int> Values { get; set; }
        string Description { get; set; }
        CardRarity Rarity { get; set; }
        List<AllHeroClass> Specialization { get; set; }
        int Level { get; set; }

        bool InCollection { get; set; }
        TargetType TargetType { get; set; }
    }
}