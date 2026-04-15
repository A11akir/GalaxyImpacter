using System.Collections.Generic;

namespace Feature.GoogleSheets
{
    public class HeroStatsConfig
    {
        public string HeroName;
        public int Health;
        public int HeroPowerCost;
    }
    
    [System.Serializable]
    public class SpellStatsConfig
    {
        public string Name;
        public int Cost;
        public List<int> Values;
        public string Description;
        public string Rarity;
        public List<string> Specialization;
        public int Level;
    }
    
    public class MinionStatsConfig
    {
        public string Name;
        public int Cost;
        public List<int> Values;
        public List<ISpellStatsData> SpellsList;
        public List<string> SpellNames = new List<string>();
        public string Rarity;
        public List<string> Specialization;
        public int Level;
        public int Health;
        public int Chakra;
        public int HandCardCount;
        
    }
}