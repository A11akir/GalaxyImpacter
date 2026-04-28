using System.Collections.Generic;
using Feature.Hero;
using NUnit.Framework;

namespace Feature.GoogleSheets
{
    public class AllGameConfig
    {
        public List<HeroStatsConfig> HeroStats;
        public List<SpellStatsConfig> AllSpellStats;
        public List<MinionStatsConfig> AllMinionStats;
        public List<ItemStatsConfig> AllItemStats;
    }
}